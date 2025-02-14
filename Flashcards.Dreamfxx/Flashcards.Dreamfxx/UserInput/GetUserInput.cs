using Flashcards.Dreamfxx.Validation;

namespace Flashcards.Dreamfxx.UserInput;
public class GetUserInput
{
    public static string GetUserString(string message)
    {




        (message);
        string? result = Console.ReadLine();
        if (Validations.ValidateString(message))
        {
            return result;
        }
        return null;
    }
}