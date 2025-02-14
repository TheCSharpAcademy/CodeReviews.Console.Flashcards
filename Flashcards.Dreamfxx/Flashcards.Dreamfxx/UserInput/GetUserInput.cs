using Flashcards.Dreamfxx.Validation;
using Spectre.Console;

namespace Flashcards.Dreamfxx.UserInput;
public class GetUserInput
{
    public static string GetUserString(string message)
    {
        AnsiConsole.MarkupLine(message);
        string? result = Console.ReadLine();
        if (Validations.ValidateString(message))
        {
            return result;
        }
        return null;
    }
}