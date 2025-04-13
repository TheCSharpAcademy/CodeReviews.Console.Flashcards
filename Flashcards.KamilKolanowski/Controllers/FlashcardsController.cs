using Flashcards.KamilKolanowski.Data;
using Flashcards.KamilKolanowski.Models;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Services;

internal class FlashcardsController
{

    internal static void AddFlashcard(DatabaseManager databaseManager)
    {
        var card = new Cards { StackId = 1, FlashcardTitle = "Spanish", FlashcardContent = "Buenos Dias!" };
        databaseManager.WriteTable("Cards", card);
    }
    internal static void ViewFlashcardsTable(DatabaseManager databaseManager)
    {
        var flashcardsTable = new Table();
        
        flashcardsTable.Title("[bold yellow]Flashcards[/]");
        flashcardsTable.Border(TableBorder.Rounded);
        flashcardsTable.BorderColor(Color.HotPink3);
        var flashcards = databaseManager.ReadTable<Cards>("Cards");

        flashcardsTable.AddColumn("[darkorange3_1]FlashcardId[/]");
        flashcardsTable.AddColumn("[darkorange3_1]StackId[/]");
        flashcardsTable.AddColumn("[darkorange3_1]FlashcardTitle[/]");
        flashcardsTable.AddColumn("[darkorange3_1]FlashcardContent[/]");
        flashcardsTable.AddColumn("[darkorange3_1]DateCreated[/]");
        
        foreach (var flashcard in flashcards)
        {
            flashcardsTable.AddRow(
                $"[grey69] {flashcard.FlashcardId}[/]",
                $"[grey69] {flashcard.StackId}[/]",
                $"[grey69] {flashcard.FlashcardTitle}[/]",
                $"[grey69] {flashcard.FlashcardContent}[/]",
                $"[grey69] {flashcard.DateCreated}[/]"
            );
        }
        
        flashcardsTable.Columns[0].Centered();
        flashcardsTable.Columns[1].Centered();
        flashcardsTable.Columns[2].Centered();
        flashcardsTable.Columns[3].Centered();
        flashcardsTable.Columns[4].Centered();
        
        AnsiConsole.Write(flashcardsTable);
        
        Console.WriteLine("Press any key to go back to the main menu.");
        Console.ReadKey();
    }
}