using Flashcards.nikosnick13.UI;
using Spectre.Console;
using System.Configuration;
using static System.Console;


namespace Flashcards.nikosnick13;

internal class Program
{
   static string? connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");


    static void Main(string[] args)
    {
        DataBaseManager dataBase = new();
        dataBase.CreateTables(connectionString);

        MenuManager menuManager = new();
        menuManager.ShowMainMenu();
            

    }
}
