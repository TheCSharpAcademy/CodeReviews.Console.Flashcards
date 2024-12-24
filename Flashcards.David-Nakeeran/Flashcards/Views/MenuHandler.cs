using Spectre.Console;

namespace Flashcards.Views;

class MenuHandler
{
    private readonly Dictionary<string, string> MenuOptions = new Dictionary<string, string>{
        {"viewAllStacks", "View all Stacks"},
        {"createAStack", "Create a Stack"},
        {"updateAStack", "Update a Stack"},
        {"deleteAStack", "Delete a Stack"},
        {"viewAllFlashcards", "View all Flashcards"},
        {"createAFlashcard", "Create a Flashcard"},
        {"updateAFlashcard", "Update a Flashcard"},
        {"deleteAFlashcard","Delete a Flashcard"},
        {"studySession", "Study Session"},
        {"viewStudySessionData", "View Study Session Data"},
        {"viewStudySessionSummary", "View Study Session Summary"},
        {"closeApp", "Quit Application"}
    };

    internal string ShowMenu()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[bold]Main Menu[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[green](Use arrow keys to navigate, then press enter)[/]");
        AnsiConsole.WriteLine();
        var userSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select an option:")
                .AddChoices(
                    MenuOptions.Values
                ));
        foreach (var selected in MenuOptions)
        {
            if (userSelection == selected.Value)
            {
                userSelection = selected.Key;
                break;
            }
        }
        return userSelection;
    }

    internal void WaitForUserInput()
    {
        AnsiConsole.MarkupLine("Press any key to continue.....");
        Console.ReadKey(true);
    }

    internal void ReturnToMainMenu(string input)
    {
        if (input == "0") return;
    }
}