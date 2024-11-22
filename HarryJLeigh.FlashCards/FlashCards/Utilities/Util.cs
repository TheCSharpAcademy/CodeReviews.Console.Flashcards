using Spectre.Console;

namespace FlashCards.Utilities;

public static class Util
{
    internal static void AskUserToContinue()
    {
        AnsiConsole.MarkupLine("[blue]Press any key to continue...[/]");
        Console.ReadKey();
    }
}