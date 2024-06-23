using FlashcardsProgram.Database;
using FlashcardsProgram.Flashcards;
using FlashcardsProgram.Stacks;
using Spectre.Console;

namespace FlashcardsProgram;

public class FlashcardUI(BaseRepository<FlashcardDAO> flashcardRepository)
{
    public BaseRepository<FlashcardDAO> flashcardRepo = flashcardRepository;

    public FlashcardDAO? SelectFlashcardFromList()
    {
        var flashcards = flashcardRepo.List();

        if (flashcards == null || flashcards.Count == 0)
        {
            AnsiConsole.MarkupLine(Utils.Text.Markup("No flashcards", "red"));
            Utils.ConsoleUtil.PressAnyKeyToClear();
            return null;
        }

        FlashcardDAO selectedFlashcard = AnsiConsole.Prompt(
            new SelectionPrompt<FlashcardDAO>()
            .Title("FLASHCARDS")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more)[/]")
            .AddChoices(flashcards)
            .EnableSearch()
        );

        return selectedFlashcard;
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

    public void DeleteFlashcard(int id)
    {
        bool success = flashcardRepo.Delete(id);

        if (success)
        {
            AnsiConsole.MarkupLine(
             success ? Utils.Text.Markup($"Done", "green") : Utils.Text.Markup($"Failed to delete", "red")
            );
        }
    }

    public void ManageFlashcard(FlashcardDAO flashcard, int currentStackId)
    {
        string selectedChoice = AnsiConsole.Prompt(
           new SelectionPrompt<string>()
           .Title($"Manage Flashcard\nFront: {flashcard.Front}\nBack: {flashcard.Back}")
           .AddChoices([
                ManageFlashcardMenuChoice.Back,
                ManageFlashcardMenuChoice.EditFrontBack,
                ManageFlashcardMenuChoice.DeleteFlashcard
           ])

       );

        switch (selectedChoice)
        {
            case ManageFlashcardMenuChoice.EditFrontBack:
                CreateOrUpdateFlashcard(currentStackId, flashcard.Id);
                break;
            case ManageFlashcardMenuChoice.DeleteFlashcard:
                DeleteFlashcard(flashcard.Id);
                break;
            default:
                break;
        }
    }
}

public static class ManageFlashcardMenuChoice
{
    public const string EditFrontBack = "[blue]Edit[/] Front/Back text";
    public const string DeleteFlashcard = "[red]Delete[/] Flashcard";
    public const string Back = "[red]<- Go back[/]";
}