using Flashcards.DreamFXX.Validation;
namespace Flashcards.DreamFXX.UserInput;

public class UserAnswer
{
    public static string GetUserAnswer(string message)
    {
        Console.WriteLine(message);
        string result = Console.ReadLine();
        if (ValidationOfInput.ValidateString(message))
        {
            return result;
        }

        return null;
    }
}
