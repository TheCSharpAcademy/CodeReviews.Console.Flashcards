namespace Flashcards.wkktoria.UserInteractions;

internal static class UserOutput
{
    private static void Print(ConsoleColor color, string message)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    internal static void ErrorMessage(string message)
    {
        Print(ConsoleColor.Red, message);
    }

    internal static void InfoMessage(string message)
    {
        Print(ConsoleColor.White, message);
    }

    internal static void SuccessMessage(string message)
    {
        Print(ConsoleColor.Green, message);
    }
}