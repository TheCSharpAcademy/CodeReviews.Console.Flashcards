namespace Flashcards.Dreamfxx.Validation;
public class Validations
{
    public static bool ValidateString(string input)
    {
        return !string.IsNullOrEmpty(input);
    }
}
