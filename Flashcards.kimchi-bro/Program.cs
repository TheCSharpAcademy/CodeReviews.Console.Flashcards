namespace Flashcards;

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Database.Initialize();
        MainMenu.ShowMainMenu();
    }
}
