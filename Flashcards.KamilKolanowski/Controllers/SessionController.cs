using Flashcards.KamilKolanowski.Controllers;
using Flashcards.KamilKolanowski.Data;
using Flashcards.KamilKolanowski.Enums;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Controllers;

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
            case Options.DBOptions.UpdateRow:
                FlashcardsController.EditFlashcard(databaseManager);
                break;
            case Options.DBOptions.DeleteRow:
                FlashcardsController.DeleteFlashcard(databaseManager);
                break;
            case Options.DBOptions.ViewRows:
                FlashcardsController.ViewFlashcardsTable(databaseManager);
                break;
        };
    }
}