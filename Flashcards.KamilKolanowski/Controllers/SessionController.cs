using Flashcards.KamilKolanowski.Data;
using Flashcards.KamilKolanowski.Enums;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Controllers;

internal class SessionController
{
    internal void ManageFlashcards()
    {
        DatabaseManager databaseManager = new();
        FlashcardsController flashcardsController = new();

        var flashcardsChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>().AddChoices(Options.FlashcardsOptionDisplay.Values)
        );

        var selectedFlashcardChoice = Options
            .FlashcardsOptionDisplay.FirstOrDefault(x => x.Value == flashcardsChoice)
            .Key;

        switch (selectedFlashcardChoice)
        {
            case Options.DBOptions.AddRow:
                flashcardsController.AddFlashcard(databaseManager);
                break;
            case Options.DBOptions.UpdateRow:
                flashcardsController.EditFlashcard(databaseManager);
                break;
            case Options.DBOptions.DeleteRow:
                flashcardsController.DeleteFlashcard(databaseManager);
                break;
            case Options.DBOptions.ViewRows:
                flashcardsController.ViewFlashcardsTable(databaseManager);
                break;
        }
        ;
    }

    internal void ManageStacks()
    {
        DatabaseManager databaseManager = new();
        StacksController stacksController = new();

        var stacksChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>().AddChoices(Options.StacksOptionDisplay.Values)
        );

        var selectedStacksChoice = Options
            .StacksOptionDisplay.FirstOrDefault(x => x.Value == stacksChoice)
            .Key;

        switch (selectedStacksChoice)
        {
            case Options.DBOptions.AddRow:
                stacksController.AddNewStack(databaseManager);
                break;
            case Options.DBOptions.UpdateRow:
                stacksController.EditStack(databaseManager);
                break;
            case Options.DBOptions.DeleteRow:
                stacksController.DeleteStack(databaseManager);
                break;
            case Options.DBOptions.ViewRows:
                stacksController.ViewStacksTable(databaseManager);
                break;
        }
    }

    internal void ManageStudySession(string studyChoice)
    {
        DatabaseManager databaseManager = new();
        StudySession studySession = new();

        if (studyChoice == "study")
        {
            studySession.StartStudy(databaseManager);
        }
        else if (studyChoice == "view")
        {
            studySession.ViewStudySessions(databaseManager);
        }
    }
}
