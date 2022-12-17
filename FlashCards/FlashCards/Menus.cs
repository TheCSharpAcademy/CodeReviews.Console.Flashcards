namespace FlashCards;
public class Menus
{
    public static void Begin()
    {
        Console.WriteLine("Welcome to Flash Cards Application");
        Console.WriteLine($"Database location:{Configurator.connectionString}");
        Console.WriteLine("---------------------------------------------\n");
        Main();
    }

    public static void Main()
    {
        Console.WriteLine();
    }
}