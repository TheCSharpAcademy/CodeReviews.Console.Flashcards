namespace Flashcards.Dreamfxx.Validation;
public class Validations
{
    public static bool ValidateString(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }
        return true;
    }
}
