using Spectre.Console;

namespace Flashcards.Helpers;

public static class ConsoleHelper
{
    // Method for string input with validation
    public static string PromptWithValidation(string message, string errorMessage)
    {
        while (true)
        {
            var input = AnsiConsole.Ask<string>(message);
            if (!string.IsNullOrWhiteSpace(input)) return input;
            AnsiConsole.MarkupLine(errorMessage);
        }
    }

    // Method for integer input with validation
    public static int PromptForInt(string message, string errorMessage)
    {
        while (true)
        {
            if (int.TryParse(AnsiConsole.Ask<string>(message), out var result) && result > 0) return result;
            AnsiConsole.MarkupLine(errorMessage);
        }
    }

    // Method for custom validation function
    public static T PromptWithCustomValidation<T>(string message, Func<T, bool> isValid, string errorMessage)
    {
        while (true)
        {
            try
            {
                var input = AnsiConsole.Ask<T>(message);
                if (isValid(input)) return input;
            }
            catch
            {
                // Ignore invalid parse attempts, as the error will be displayed below
            }

            AnsiConsole.MarkupLine(errorMessage);
        }
    }
}