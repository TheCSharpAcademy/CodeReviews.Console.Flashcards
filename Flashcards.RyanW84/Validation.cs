using Spectre.Console;

namespace Flashcards.RyanW84
{
    internal class Validation
    {
        internal static int ValidateInt(string input, string message)
        {
            int output = 0;

            while (!int.TryParse(input, out output) || Convert.ToInt32(input) < 0)
            {
                input = AnsiConsole.Ask<string>("Invalid number: " + message);
            }

            return output;
        }
    }
}
