using Spectre.Console;

namespace FlashCards.Utilities;

public static class Util
{
    internal static void AskUserToContinue()
    {
        AnsiConsole.MarkupLine("[blue]Press any key to continue...[/]");
        Console.ReadKey();
    }

    internal static bool ReturnToMenu()
    {
        AnsiConsole.MarkupLine("Type [yellow]'0'[/] to return to menu or press any key to continue: ");
        string input = Console.ReadLine();

        if (input == "0") return true;

        return false;
    }
}