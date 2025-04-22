using Flashcards.KamilKolanowski.Data;
using Flashcards.KamilKolanowski.Handlers;
using Flashcards.KamilKolanowski.Helpers;
using Flashcards.KamilKolanowski.Models;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Controllers;

internal class FlashcardsController
{
    internal void AddFlashcard(DatabaseManager databaseManager)
    {
        UserInputHandler userInputHandler = new();
        var stacks = databaseManager.ReadStacks().Select(s => (s.StackId, s.StackName)).ToList();
        var newCard = userInputHandler.CreateFlashcard(stacks);
        var flashcards = databaseManager.ReadCards(newCard.StackId).ToList();

        if (VerifyIfFlashcardExists(flashcards, newCard.FlashcardTitle))
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[red]Flashcard already exists[/]");
            AnsiConsole.MarkupLine("Press any key to go back to Main Menu");
            Console.ReadKey();
            return;
        }

        databaseManager.AddCard(newCard);
        InformUserWithStatus("added");
    }

    internal void EditFlashcard(DatabaseManager databaseManager)
    {
        var updateCardDto = new UpdateCardDto();

        updateCardDto.StackId = StackChoice.GetStackChoice(databaseManager);

        var (flashcardId, flashcardTitle) = GetFlashcardChoice(
            databaseManager,
            updateCardDto.StackId
        );
        updateCardDto.FlashcardId = flashcardId;
        updateCardDto.FlashcardTitle = flashcardTitle;

        if (string.IsNullOrEmpty(updateCardDto.FlashcardTitle))
        {
            return;
        }

        updateCardDto.ColumnToUpdate = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose the column to edit")
                .AddChoices(["StackName", "FlashcardTitle", "FlashcardContent"])
        );

        if (updateCardDto.ColumnToUpdate == "StackName")
        {
            AnsiConsole.MarkupLine("You're choosing new Stack for this flashcard.");
            updateCardDto.NewValue = StackChoice.GetStackChoice(databaseManager).ToString();
        }
        else
        {
            updateCardDto.NewValue = AnsiConsole.Prompt(
                new TextPrompt<string>($"Provide new value for {updateCardDto.ColumnToUpdate}: ")
            );
        }

        if (
            updateCardDto.ColumnToUpdate == "FlashcardTitle"
            || updateCardDto.ColumnToUpdate == "StackName"
        )
        {
            var newStackId =
                updateCardDto.ColumnToUpdate == "StackName"
                    ? int.Parse(updateCardDto.NewValue)
                    : updateCardDto.StackId;

            var newTitle =
                updateCardDto.ColumnToUpdate == "FlashcardTitle"
                    ? updateCardDto.NewValue
                    : updateCardDto.FlashcardTitle;

            var flashcardsInTargetStack = databaseManager.ReadCards(newStackId).ToList();

            if (
                VerifyIfFlashcardExists(
                    flashcardsInTargetStack,
                    newTitle,
                    updateCardDto.FlashcardId
                )
            )
            {
                Console.Clear();
                AnsiConsole.MarkupLine("[red]A flashcard with this title already exists![/]");
                AnsiConsole.MarkupLine("Press any key to go back to Main Menu.");
                Console.ReadKey();
                return;
            }
        }

        databaseManager.UpdateCards(updateCardDto);
        InformUserWithStatus("updated");
    }

    internal void DeleteFlashcard(DatabaseManager databaseManager)
    {
        var stackChoice = StackChoice.GetStackChoice(databaseManager);
        var flashcardChoice = GetFlashcardChoice(databaseManager, stackChoice).FlashcardId;

        if (flashcardChoice == 0)
        {
            return;
        }

        databaseManager.DeleteCards(stackChoice, flashcardChoice);
        InformUserWithStatus("deleted");
    }

    internal void ViewFlashcardsTable(DatabaseManager databaseManager)
    {
        var stackChoice = StackChoice.GetStackChoice(databaseManager);
        var cardDtos = GetFlashcardDtosForStack(databaseManager, stackChoice);
        var table = BuildFlashcardsTable(cardDtos);

        AnsiConsole.Write(table);

        AnsiConsole.MarkupLine("Press any key to go back to the main menu.");
        Console.ReadKey();
    }

    private (int FlashcardId, string FlashcardTitle) GetFlashcardChoice(
        DatabaseManager databaseManager,
        int stackChoice
    )
    {
        var flashcards = GetFlashcardDtosForStack(databaseManager, stackChoice)
            .Select(x => (x.FlashcardId, x.FlashcardTitle))
            .ToList();

        var flashcardTitles = flashcards.Select(x => x.FlashcardTitle).ToList();

        if (!flashcards.Any())
        {
            AnsiConsole.MarkupLine("[yellow3_1]There are no flashcards for this stack.[/]");
            AnsiConsole.MarkupLine("Press any key to go back to the main menu.");
            Console.ReadKey();
            return (0, null);
        }

        var flashcardChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose the flashcard from the list")
                .AddChoices(flashcardTitles)
        );

        var selected = flashcards.First(x => x.FlashcardTitle == flashcardChoice);
        return (selected.FlashcardId, selected.FlashcardTitle);
    }

    internal IList<CardDto> GetFlashcardDtosForStack(
        DatabaseManager databaseManager,
        int stackChoice
    )
    {
        var flashcards = databaseManager.ReadCards(stackChoice);

        return flashcards
            .Select(card => new CardDto
            {
                FlashcardId = card.FlashcardId,
                FlashcardTitle = card.FlashcardTitle,
                FlashcardContent = card.FlashcardContent,
            })
            .ToList();
    }

    private Table BuildFlashcardsTable(IList<CardDto> cardDtos)
    {
        var flashcardsTable = new Table();

        flashcardsTable.Title("[bold yellow]Flashcards[/]");
        flashcardsTable.Border(TableBorder.Rounded);
        flashcardsTable.BorderColor(Color.HotPink3);

        flashcardsTable.AddColumn("[darkorange3_1]Flashcard Id[/]");
        flashcardsTable.AddColumn("[darkorange3_1]Flashcard Title[/]");
        flashcardsTable.AddColumn("[darkorange3_1]Flashcard Content[/]");

        var idx = 1;
        foreach (var flashcard in cardDtos)
        {
            flashcardsTable.AddRow(
                $"[grey69] {idx}[/]",
                $"[grey69] {flashcard.FlashcardTitle}[/]",
                $"[grey69] {flashcard.FlashcardContent}[/]"
            );
            idx++;
        }

        foreach (var column in flashcardsTable.Columns)
        {
            column.Centered();
        }

        return flashcardsTable;
    }

    private bool VerifyIfFlashcardExists(
        IList<CardDto> existingFlashcards,
        string newFlashcardTitle,
        int? excludeId = null
    )
    {
        return existingFlashcards.Any(c =>
            c.FlashcardTitle.Equals(newFlashcardTitle, StringComparison.OrdinalIgnoreCase)
            && (!excludeId.HasValue || c.FlashcardId != excludeId.Value)
        );
    }

    private void InformUserWithStatus(string option)
    {
        Console.Clear();
        AnsiConsole.MarkupLine(
            $"[springgreen2_1]Flashcard {option} successfully.[/] \nPress any key to go back to Main Menu."
        );
        Console.ReadKey();
    }
}
