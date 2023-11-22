using Flashcards.SamGannon.UI;
using System.Configuration;

string? connectionString = ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;

if (connectionString == null )
{
    Console.WriteLine("No connection detection.");

}

MainMenu.ShowMenu();