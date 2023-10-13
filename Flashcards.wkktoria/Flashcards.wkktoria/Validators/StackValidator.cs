namespace Flashcards.wkktoria.Validators;

internal static class StackValidator
{
    internal static bool CheckName(string name)
    {
        return name.Length is > 0 and <= 25;
    }
}