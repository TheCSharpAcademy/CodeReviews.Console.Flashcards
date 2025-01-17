namespace Flashcards.DreamFXX.Validation;
public class ValidationOfInput
{
    public static bool ValidateString(string input)
    {
        while (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }
        return true;
    }
}
