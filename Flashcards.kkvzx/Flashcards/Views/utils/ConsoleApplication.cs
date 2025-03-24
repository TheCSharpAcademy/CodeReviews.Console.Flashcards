using Spectre.Console;

namespace Flashcards.Views.utils;

public class ConsoleApplication
{
    public void ShowMessage(string message)
    {
        AnsiConsole.MarkupLine(message);
    }
    
    public void ShowSuccess(string message)
    {
        AnsiConsole.MarkupLine($"[green]{message}[/]");
    }
    
    public void ShowError(string message)
    {
        AnsiConsole.MarkupLine($"[red]{message}[/]");
    }
    
    public void PressKeyToContinue()
    {
        AnsiConsole.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Clear();
    }

    public void Clear()
    {
        AnsiConsole.Clear();
    }
}