namespace Flashcards;

class Helpers
{
    public static void ClearConsole()
    {
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();
    }
}