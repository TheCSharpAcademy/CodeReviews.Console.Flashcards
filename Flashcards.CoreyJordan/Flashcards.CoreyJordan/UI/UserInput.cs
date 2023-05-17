namespace Flashcards.CoreyJordan.UI;
internal static class UserInput
{
    internal static string GetString(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine()!;
    }
}
