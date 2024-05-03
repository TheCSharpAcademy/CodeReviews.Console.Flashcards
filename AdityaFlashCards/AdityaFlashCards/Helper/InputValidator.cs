namespace AdityaFlashCards.Helper;

internal class InputValidator
{
    internal static bool IsGivenInputInteger(string input)
    {
        if (int.TryParse(input, out _))
            return true;
        else
            return false;
    }
}