using FlashCards.Database;
using FlashCards.Controllers;
using Spectre.Console;

namespace FlashCards;

public class Program()
{
    public static void Main(string[] args)
    {
        bool initializationSuccessful = GeneralDBHelper.InitializeDatabase();

        if (initializationSuccessful) MenuController.RunMenuLoop();

        AnsiConsole.WriteLine("\nPress any key to exit the program.");
        AnsiConsole.Console.Input.ReadKey(false);
    }
}
