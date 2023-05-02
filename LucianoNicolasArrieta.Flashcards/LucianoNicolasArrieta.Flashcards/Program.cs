using LucianoNicolasArrieta.Flashcards;
using LucianoNicolasArrieta.Flashcards.Persistence;

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Flashcards App!");

        DBManager dbManager = new DBManager();
        dbManager.DBInit();

        Menu menu = new Menu();
        menu.RunMainMenu();
    }
}