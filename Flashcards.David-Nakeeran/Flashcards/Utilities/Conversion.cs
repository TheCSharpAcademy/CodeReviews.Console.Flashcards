using Spectre.Console;

namespace Flashcards.Utilities;

class Conversion
{
    internal int ParseInt(string? input, string message)
    {
        int cleanNum;
        while (!int.TryParse(input, out cleanNum))
        {
            input = AnsiConsole.Ask<string>(message);
        }
        return cleanNum;
    }
}