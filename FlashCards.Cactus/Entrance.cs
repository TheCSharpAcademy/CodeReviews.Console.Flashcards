using FlashCards.Cactus;

namespace FlashCards;
public class Entrance
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Initialize FlashCard application, please wait for a while...");
        Application app = new Application();
        app.Run();
    }
}