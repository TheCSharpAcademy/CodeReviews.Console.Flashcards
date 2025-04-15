using Flashcards.KamilKolanowski.Data;
using Flashcards.KamilKolanowski.Models;
using Flashcards.KamilKolanowski.Handlers;
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
        
        databaseManager.WriteTable("Cards", newCard);
        
        InformUserWithStatus("added");
    }

    internal static void EditFlashcard(DatabaseManager databaseManager)
    {
        var stackChoice = GetStackChoice(databaseManager);
        var flashcardChoice = GetFlashcardChoice(databaseManager, stackChoice);

        var columnToUpdate = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose the column to edit")
                .AddChoices(["StackName", "FlashcardTitle", "FlashcardContent"]));

        
        if (columnToUpdate == "StackName")
        {
            Console.WriteLine("You're choosing new Stack for this flashcard.");
            var newStack = GetStackChoice(databaseManager);
            
            databaseManager.UpdateCards(stackChoice, flashcardChoice, columnToUpdate, newStack);
        }

        else
        {
            var newValue = AnsiConsole.Prompt(
                new TextPrompt<string>($"Provide new value for {columnToUpdate}: "));
        
            databaseManager.UpdateCards(stackChoice, flashcardChoice, columnToUpdate, newValue);
        }

        InformUserWithStatus("updated");
    }

    internal static void DeleteFlashcard(DatabaseManager databaseManager)
    {
        var stackChoice = GetStackChoice(databaseManager);
        var flashcardChoice = GetFlashcardChoice(databaseManager, stackChoice);
        
        databaseManager.DeleteCards(stackChoice, flashcardChoice);

        InformUserWithStatus("deleted");
    }
    internal static void ViewFlashcardsTable(DatabaseManager databaseManager)
    {
        var stackChoice = GetStackChoice(databaseManager);
        var cardDtos = GetFlashcardDtosForStack(databaseManager, stackChoice);
        var table = BuildFlashcardsTable(cardDtos);
        
        AnsiConsole.Write(table);
        
        Console.WriteLine("Press any key to go back to the main menu.");
        Console.ReadKey();
    }

    private static string GetStackChoice(DatabaseManager databaseManager)
    {
        var stacks = databaseManager.ReadStacks()
            .Select(s => (s.StackId, s.StackName))
            .ToList();
        
        var stackNames = stacks.Select(x => x.Item2).ToList();
        
        var stackChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose the Stack from the list")
                .AddChoices(stackNames));
        
        return stackChoice;
    }

    private static string GetFlashcardChoice(DatabaseManager databaseManager, string stackChoice)
    {
        var flashcardTitle = GetFlashcardDtosForStack(databaseManager, stackChoice).Select(x => x.FlashcardTitle).ToList();

        if (flashcardTitle.Count == 0)
        {
            Console.WriteLine("There are no flashcards for this stack.");
            Console.WriteLine("Press any key to go back to the main menu.");
            Console.ReadKey();
            return "";
            
            // fix the bugs here - operate when there's no flashcard in the stack
        }
        else
        {
            var flashcardChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose the flashcard from the list")
                    .AddChoices(flashcardTitle));

            return flashcardChoice;
        }
        
    }
    
    private static List<CardDto> GetFlashcardDtosForStack(DatabaseManager databaseManager, string stackChoice)
    {
        var flashcards = databaseManager.ReadCards(stackChoice);

        return flashcards.Select(card => new CardDto    
        {
            StackId = card.StackId,
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
        
        flashcardsTable.AddColumn("[darkorange3_1]FlashcardId[/]");
        flashcardsTable.AddColumn("[darkorange3_1]StackId[/]");
        flashcardsTable.AddColumn("[darkorange3_1]FlashcardTitle[/]");
        flashcardsTable.AddColumn("[darkorange3_1]FlashcardContent[/]");
        
        var idx = 1;
        foreach (var flashcard in cardDtos)
        {
            flashcardsTable.AddRow(
                $"[grey69] {idx}[/]",
                $"[grey69] {flashcard.StackId}[/]",
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

    private static void InformUserWithStatus(string option)
    {
        Console.Clear();
        Console.WriteLine($"Flashcard {option} successfully. \nPress any key to go back to Main Menu.");
        Console.ReadKey();
    }
}