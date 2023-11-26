namespace Flashcards.Utils;

public static class Validate
{
    public static bool IsValidNumber(string number)
    {
        return int.TryParse(number, out _);
    }

    public static bool IsValidString(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }
}