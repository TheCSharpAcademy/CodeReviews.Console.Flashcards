using Spectre.Console;

namespace FlashcardsProgram;

public class Menu
{
    public const string CREATE_SESSION = "Start [bold][green]Study[/][/]";
    public const string VIEW_SESSIONS = "View [bold][green]past study sessions[/][/]";
    public const string MANAGE_STACKS_CARDS = "Manage [bold][green]stacks & flashcards[/][/]";
    public const string CREATE_STACK = "Create [bold][green]new stack[/][/]";
    public const string VIEW_REPORT = "View [bold][green] report[/][/]: avg score % [grey](/month/stack)[/]";
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
                MANAGE_STACKS_CARDS,
                CREATE_STACK,
                VIEW_REPORT
            )
        );
    }
}