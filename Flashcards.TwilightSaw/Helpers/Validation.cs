using Spectre.Console;

namespace Flashcards.TwilightSaw.Helpers;

public static class Validation
{
    public static string Validate(Action action)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            return e.Message;
        }
        return "Executed successfully";
    }

    public static void EndMessage(string? message)
    {
        if (message != null)
        {
            AnsiConsole.MarkupLine($"[olive]{message}[/]");
            AnsiConsole.Markup($"[grey]Press any key to continue.[/]");
            Console.ReadKey(intercept: true);
        }
        Console.Clear();
    }
}