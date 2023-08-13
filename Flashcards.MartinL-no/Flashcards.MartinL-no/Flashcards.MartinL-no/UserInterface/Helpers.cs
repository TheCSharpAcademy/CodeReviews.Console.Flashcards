namespace Flashcards.MartinL_no.UserInterface;

internal class Helpers
{
    static public void ShowMessage(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        Thread.Sleep(2500);
    }

    static public void ShowLine()
    {
        Console.WriteLine("---------------------------------");
    }

    static public string Ask(string message)
    {
        Console.Write(message);
        return Console.ReadLine();
    }
}

