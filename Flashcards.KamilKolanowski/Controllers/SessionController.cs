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

    internal static void ManageStacks()
    {
        DatabaseManager databaseManager = new();

        var stacksChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices(Options.StacksOptionDisplay.Values));
        
        var selectedStacksChoice = Options.StacksOptionDisplay
            .FirstOrDefault(x => x.Value == stacksChoice).Key;

        switch (selectedStacksChoice)
        {
            case Options.DBOptions.AddRow:
                // StacksController.AddStack(databaseManager);
                break;
            case Options.DBOptions.UpdateRow:
                // StacksController.EditStack(databaseManager);
                break;
            case Options.DBOptions.DeleteRow:
                // StacksController.DeleteStack(databaseManager);
                break;
            case Options.DBOptions.ViewRows:
                StacksController.ViewStacksTable(databaseManager);
                break;
        }
    }
}