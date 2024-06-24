using FlashcardsProgram.Flashcards;
using Spectre.Console;

namespace FlashcardsProgram;

public class FlashcardsController(FlashcardsRepository flashcardRepository)
{
    public FlashcardsRepository flashcardRepo = flashcardRepository;

    public FlashcardDAO? SelectFlashcardFromList(int stackId)
    {
        var flashcards = flashcardRepo.GetByStackId(stackId);

        if (flashcards == null || flashcards.Count == 0)
        {
            AnsiConsole.MarkupLine(Utils.Text.Markup("No flashcards", "red"));
            Utils.ConsoleUtil.PressAnyKeyToClear();
            return null;
        }

        FlashcardUiDto selectedFlashcard = AnsiConsole.Prompt(
            new SelectionPrompt<FlashcardUiDto>()
            .Title("Flashcards")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more)[/]")
            .AddChoices([
                new FlashcardUiDto(
                    new FlashcardDAO(-1, "[red]<- Go back[/]", "", -1),
                    ""
                ),
                .. flashcards.Select((card, index) => new FlashcardUiDto(card, (index + 1).ToString()))
            ])
            .EnableSearch()
        );

        if (selectedFlashcard.Card.Id == -1)
        {
            return null;
        }

        return selectedFlashcard.Card;
    }

    public FlashcardDAO? CreateOrUpdateFlashcard(int stackId, int? id = null)
    {
        string front;
        string back;

        AnsiConsole.MarkupLine("\n[blue]Flashcard[/]\n");

        front = AnsiConsole.Ask<string>("\nFront? ");

        back = AnsiConsole.Ask<string>("\nBack? ");

        var upsertedFlashcard = id.HasValue ?
            flashcardRepo.Update(id.Value, new UpdateFlashcardDTO(front, back)) :
            flashcardRepo.Create(new CreateFlashcardDTO(front, back, stackId));

        if (upsertedFlashcard == null)
        {
            AnsiConsole.MarkupLine(Utils.Text.Markup($"Could not perform operation", "red"));
            return null;
        }

        AnsiConsole.MarkupLine(
            Utils.Text.Markup($"Done", "green")
        );

        return upsertedFlashcard;
    }

    private void DeleteFlashcard(int id)
    {
        bool success = flashcardRepo.Delete(id);

        if (success)
        {
            AnsiConsole.MarkupLine(
             success ? Utils.Text.Markup($"Done", "green") : Utils.Text.Markup($"Failed to delete", "red")
            );
        }
    }

    public bool ManageFlashcard(FlashcardDAO flashcard, int currentStackId)
    {
        bool showMenu = true;

        do
        {
            var fetchedFlashcard = flashcardRepo.GetById(flashcard.Id);
            Console.Clear();

            string selectedChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title($"Manage Flashcard \n(\tFront: {fetchedFlashcard.Front}\tBack: {fetchedFlashcard.Back}\t)")
                .AddChoices([
                    ManageFlashcardMenuChoice.Back,
                    ManageFlashcardMenuChoice.EditFrontBack,
                    ManageFlashcardMenuChoice.DeleteFlashcard
                ])
            );

            switch (selectedChoice)
            {
                case ManageFlashcardMenuChoice.EditFrontBack:
                    CreateOrUpdateFlashcard(currentStackId, fetchedFlashcard.Id);
                    showMenu = true;
                    break;
                case ManageFlashcardMenuChoice.DeleteFlashcard:
                    DeleteFlashcard(fetchedFlashcard.Id);
                    showMenu = false;
                    break;
                case ManageFlashcardMenuChoice.Back:
                    showMenu = false;
                    break;
                default:
                    return true;
            }
        }
        while (showMenu);

        return true;
    }

    public void DisplayCard(string stackName, FlashcardDTO flashcard, int order, bool showBack = false)
    {
        string backContents = showBack ? $"[bold][green]{flashcard.Back}[/][/]" : "";
        string frontContents = !showBack ? $"[blue]{flashcard.Front}[/]" : "";
        string frontOrBack = showBack ? "[green]Answer[/]" : "Question";
        string contents = $"{frontContents}{backContents}";

        var table = new Table();

        table.AddColumn(new TableColumn($"{stackName} {frontOrBack} #{order}").Centered().Width(30));
        table.AddRow(contents);

        AnsiConsole.Write(table);
    }
}

public static class ManageFlashcardMenuChoice
{
    public const string EditFrontBack = "[blue]Edit[/] Front/Back text";
    public const string DeleteFlashcard = "[red]Delete[/] Flashcard";
    public const string Back = "[red]<- Go back[/]";
}

internal class FlashcardUiDto(FlashcardDAO card, string order)
{
    public FlashcardDAO Card = card;
    public string Order = order;

    public override string ToString()
    {
        return $"#{Order}\t{Card.Front}\t{Card.Back}";
    }
}