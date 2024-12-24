using Spectre.Console;

namespace Flashcards.Utilities;

class Validation
{
    internal string CheckInputNullOrWhitespace(string message, string? input)
    {
        while (string.IsNullOrWhiteSpace(input))
        {
            input = AnsiConsole.Ask<string>(message);
        }
        return input;
    }
}

