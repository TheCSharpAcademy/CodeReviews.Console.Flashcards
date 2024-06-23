using Spectre.Console;

namespace FlashcardsProgram;

public class Menu
{
    public const string CREATE_SESSION = "Start Study session";
    public const string VIEW_SESSIONS = "View past study sessions";
    public const string MANAGE_STACK = "Manage stack";
    public const string CREATE_STACK = "Create new stack";
    public const string EXIT = "[red]Exit[/]";

    public static string ShowMainMenu()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .EnableSearch()
            .Title("\n\t[green]Menu[/]")
            .AddChoices(
                CREATE_SESSION,
                VIEW_SESSIONS,
                MANAGE_STACK,
                CREATE_STACK,
                EXIT
            )
        );
    }
}