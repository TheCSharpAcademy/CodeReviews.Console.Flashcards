using Spectre.Console;

internal class MainMenu
{
    private static readonly Dictionary<string, Action> _menuActions = new()
    {
        { "Manage Stacks", StacksMenu.ShowStacksMenu },
        { "Manage Flashcards", FlashcardsMenu.ShowFlashcardsMenu },
        { "Study", StudySession.ShowStudyMenu },
        { "[yellow]Generate new flashcards[/]", () =>
            {
                try
                {
                    FlashcardGenerator.CreateNewFlashcardsAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    DisplayErrorHelpers.GeneralError(ex);
                }
            } 
        },
        { "[yellow]Generate random sessions[/]", SessionsGenerator.GenerateRandomSessions },
        { "Exit the app", () =>
            {
                Console.Clear();
                AnsiConsole.MarkupLine("[yellow]Goodbye![/]");
                Environment.Exit(0);
            }
        }
    };

    internal static void ShowMainMenu()
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Choose an action: ")
                .PageSize(10)
                .AddChoices(_menuActions.Keys));

            _menuActions[choice]();
        }
    }
}
