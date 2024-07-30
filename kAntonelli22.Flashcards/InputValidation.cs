using Spectre.Console;

namespace Flashcards;
internal class InputValidation
{
    static string CleanString()
    {
        string input = Console.ReadLine() ?? "";
        input.Trim().ToLower();
        
        if (input == "0")
            Environment.Exit(0);
        else if (input == "menu")
            Program.MainMenu();

        
        AnsiConsole.MarkupLine("[yellow]CleanString is incomplete[/]");
        OutputUtilities.ReturnToMenu("");
        return "";
    } // end of CleanString Method
}
