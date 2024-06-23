using Spectre.Console;

namespace FlashcardsProgram;

public class Menu
{
    public const string CREATE_SESSION = "Start [bold][green]Study session[/][/]";
    public const string VIEW_SESSIONS = "View [bold][green]past study sessions[/][/]";
    public const string MANAGE_STACK = "View/Edit/Delete [bold][green]stacks[/][/]";
    public const string CREATE_STACK = "Create [bold][green]new stack[/][/]";
    public const string EXIT = "[red]Exit[/]";

    public static string ShowMainMenu()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .EnableSearch()
            .Title("\n\t[green]Menu[/]")
            .AddChoices(
                EXIT,
                CREATE_SESSION,
                VIEW_SESSIONS,
                MANAGE_STACK,
                CREATE_STACK
            )
        );
    }
}