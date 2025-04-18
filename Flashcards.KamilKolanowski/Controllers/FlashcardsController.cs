using Flashcards.KamilKolanowski.Data;
using Flashcards.KamilKolanowski.Models;
using Flashcards.KamilKolanowski.Handlers;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Controllers;

internal class FlashcardsController
{
    internal static void AddFlashcard(DatabaseManager databaseManager)
    {
        
        var stacks = databaseManager.ReadStacks()
                                                            .Select(s => (s.StackId, s.StackName))
                                                            .ToList();
        
        var newCard = UserInputHandler.CreateFlashcard(stacks);
        
        var createCardDto = new CreateCardDto()
        {
            StackId = newCard.StackId,
            FlashcardTitle = newCard.FlashcardTitle,
            FlashcardContent = newCard.FlashcardContent
        };

        VerifyIfFlashcardExists(databaseManager, newCard.StackId, createCardDto.FlashcardTitle);
        
        databaseManager.AddCard(createCardDto);
        
    }

    internal static void EditFlashcard(DatabaseManager databaseManager)
    {
        var updateCardDto = new UpdateCardDto();
        
        updateCardDto.StackId = GetStackChoice(databaseManager);
        updateCardDto.FlashcardTitle = GetFlashcardChoice(databaseManager, updateCardDto.StackId);

        if (string.IsNullOrEmpty(updateCardDto.FlashcardTitle))
        {
            return;
        }
        
        updateCardDto.ColumnToUpdate = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose the column to edit")
                .AddChoices(["StackName", "FlashcardTitle", "FlashcardContent"]));

        
        if (updateCardDto.ColumnToUpdate == "StackName")
        {
            AnsiConsole.MarkupLine("You're choosing new Stack for this flashcard.");

            updateCardDto.NewValue = GetStackChoice(databaseManager).ToString();
            databaseManager.UpdateCards(updateCardDto);
        }

        else
        {
            updateCardDto.NewValue = AnsiConsole.Prompt(
                new TextPrompt<string>($"Provide new value for {updateCardDto.ColumnToUpdate}: "));

            databaseManager.UpdateCards(updateCardDto);
        }

        InformUserWithStatus("updated");
    }

    internal static void DeleteFlashcard(DatabaseManager databaseManager)
    {
        var stackChoice = GetStackChoice(databaseManager);
        var flashcardChoice = GetFlashcardChoice(databaseManager, stackChoice);

        if (flashcardChoice.IsNullOrEmpty())
        {
            return;
        }
        
        databaseManager.DeleteCards(stackChoice, flashcardChoice);
        InformUserWithStatus("deleted");
    }
    internal static void ViewFlashcardsTable(DatabaseManager databaseManager)
    {
        var stackChoice = GetStackChoice(databaseManager);
        var cardDtos = GetFlashcardDtosForStack(databaseManager, stackChoice);
        var table = BuildFlashcardsTable(cardDtos);
        
        AnsiConsole.Write(table);
        
        AnsiConsole.MarkupLine("Press any key to go back to the main menu.");
        Console.ReadKey();
    }

    private static int GetStackChoice(DatabaseManager databaseManager)
    {
        var stacks = databaseManager.ReadStacks()
            .Select(s => (s.StackId, s.StackName))
            .ToList();
        
        var stackNames = stacks.Select(x => x.Item2).ToList();
        
        var stackChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose the Stack from the list")
                .AddChoices(stackNames));
        
        return stacks.First(x => x.Item2 == stackChoice).Item1;
    }

    private static string GetFlashcardChoice(DatabaseManager databaseManager, int stackChoice)
    {
        var flashcardTitle = GetFlashcardDtosForStack(databaseManager, stackChoice).Select(x => x.FlashcardTitle).ToList();
        
        if (!flashcardTitle.Any())
        {
            AnsiConsole.MarkupLine("[yellow3_1]There are no flashcards for this stack.[/]");
            AnsiConsole.MarkupLine("Press any key to go back to the main menu.");
            Console.ReadKey();
            return "";
        }
        
        var flashcardChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose the flashcard from the list")
                .AddChoices(flashcardTitle));

        return flashcardChoice;

    }
    
    private static List<CardDto> GetFlashcardDtosForStack(DatabaseManager databaseManager, int stackChoice)
    {
        var flashcards = databaseManager.ReadCards(stackChoice);

        return flashcards.Select(card => new CardDto    
        {
            FlashcardTitle = card.FlashcardTitle,
            FlashcardContent = card.FlashcardContent
        }).ToList();
    }

    private static Table BuildFlashcardsTable(List<CardDto> cardDtos)
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

    private static void VerifyIfFlashcardExists(DatabaseManager databaseManager, int stackChoice, string newFlashcardTitle)
    {
        var flashcards = databaseManager.ReadCards(stackChoice);

        if (flashcards.Select(x => x.FlashcardTitle.ToLower()).Contains(newFlashcardTitle.ToLower()))
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[red]Flashcard already exists[/]");
            AnsiConsole.MarkupLine("Press any key to go back to Main Menu");
            Console.ReadKey();
        }
        else
        {
            InformUserWithStatus("added");
        }
    }

    private static void InformUserWithStatus(string option)
    {
        Console.Clear();
        AnsiConsole.MarkupLine($"[springgreen2_1]Flashcard {option} successfully.[/] \nPress any key to go back to Main Menu.");
        Console.ReadKey();
    }
}