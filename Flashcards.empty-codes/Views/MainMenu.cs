using Spectre.Console;

namespace Flashcards.empty_codes.Views;

internal class MainMenu
{
    public bool GetMainMenu()
    {
        Console.Clear();
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an [green]option below[/]?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal your choices)[/]")
                .AddChoices(new[] {
                    "Manage Stacks",
                    "Manage Study Sessions", "Exit",
                }));

        switch (choice)
        {
            case "Manage Stacks":
                var stackMenu = new StackMenu();
                stackMenu.GetStackMenu();
                break;
            case "Manage Study Sessions":
                var sessionMenu = new StudySessionMenu();
                sessionMenu.GetStudySessionMenu();
                break;
            case "Exit":
                return false;
            default:
                AnsiConsole.WriteLine("Invalid selection. Please try again.");
                break;
        }
        return true;
    }    
}