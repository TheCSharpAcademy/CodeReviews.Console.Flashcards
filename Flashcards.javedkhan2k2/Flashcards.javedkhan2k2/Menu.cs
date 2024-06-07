using Flashcards.Models;
using Spectre.Console;

namespace Flashcards;

internal static class Menu
{
    static string[] MainMenuChoices = [
                    "Stack",
                    "Flashcard",
                    "Study",
                    "Report",
                    "Exit"
                ];
    public static Dictionary<string, string[]> MainMenus =  new Dictionary<string, string[]>
    {
        {"Stack", ["Add a Stack", "Delete a Stack", "Back to Main Menu"]},
        {"Flashcard", ["Add a Flash Card", "Delete a Flash Card", "Back to Main Menu"]},
        {"Study", ["Start Study Session", "View Study Sessions by Stack", "Back to Main Menu"]},
        {"Report", ["Average Score Yearly Report for one Stack", "Average Score Yearly Report All Stacks", "Back to Main Menu"]},
    };

    internal static string CancelOperation = $"[maroon]Cancel the Operation[/]";
    internal static string GetMainMenuChoices()
    {
        return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please Select an Option from the Menu Below")
                    .PageSize(10)
                    .AddChoices(MainMenuChoices)
            );
    }

    internal static string GetStackMenu(IEnumerable<Stack>? stacks)
    {
        return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please Select a Stack to Delete or select [maroon]Cancel the Operation[/] to go back to main Menu")
                    .PageSize(10)
                    .AddChoices(Helpers.ConvertStacksToArray(stacks).Append(CancelOperation))
            );
    }

    internal static string GetFlashCardsChoices(IEnumerable<Flashcard>? flashcards)
    {
        return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please Select a Stack to Delete or select [maroon]Cancel the Operation[/] to go back to main Menu")
                    .PageSize(10)
                    .AddChoices(Helpers.ConvertFlashcardsToArray(flashcards).Append(CancelOperation))
            );
    }

    internal static string GetSubMenu(string choice)
    {
        return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please Select an Option from the Menu Below")
                    .PageSize(10)
                    .AddChoices(MainMenus[choice])
            );
    }
}