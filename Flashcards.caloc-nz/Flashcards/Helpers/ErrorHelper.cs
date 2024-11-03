using Spectre.Console;

namespace Flashcards.Helpers;

public static class ErrorHelper
{
    // Display error with message and exception, with optional stack trace
    public static void DisplayError(string message, Exception ex, bool showStackTrace = false)
    {
        AnsiConsole.MarkupLine($"[red]{message}: {ex.Message}[/]");

        if (showStackTrace) AnsiConsole.WriteException(ex, ExceptionFormats.ShortenPaths);
    }

    // Overload for displaying error with just a message
    public static void DisplayError(string message)
    {
        AnsiConsole.MarkupLine($"[red]{message}[/]");
    }
}