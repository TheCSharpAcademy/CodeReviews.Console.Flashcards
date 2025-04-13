using Flashcards.KamilKolanowski.Data;
using Flashcards.KamilKolanowski.Enums;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Services;

internal class SessionController
{
    internal static void ManageFlashcards()
    {
        DatabaseManager databaseManager = new();
        
        var flashcardsChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices(Options.FlashcardsOptionDisplay.Values));
        
        var selectedFlashcardChoice = Options.FlashcardsOptionDisplay
            .FirstOrDefault(x => x.Value == flashcardsChoice).Key;

        switch (selectedFlashcardChoice)
        {
            case Options.DBOptions.AddRow:
                FlashcardsController.AddFlashcard(databaseManager);
                break;
            case Options.DBOptions.DeleteRow:
                Console.WriteLine("Deleting Flashcard");
                break;
            case Options.DBOptions.UpdateRow:
                Console.WriteLine("Updating Flashcard");
                break;
            case Options.DBOptions.ViewRows:
                FlashcardsController.ViewFlashcardsTable(databaseManager);
                break;
        };
    }
}