using Spectre.Console;
using cacheMe512.Flashcards.Models;

namespace cacheMe512.Flashcards;

internal class Utilities
{
    public static void DisplayMessage(string message, string color = "yellow")
    {
        AnsiConsole.MarkupLine($"[{color}]{message}[/]");
    }

    public static bool ConfirmDeletion(Stack stack)
    {
        var confirm = AnsiConsole.Confirm($"Are you sure you want to delete {stack.Name}?" +
            $"[red]This will delete the stack and all of its associated flashcards.[/]");

        return confirm;
    }
}
