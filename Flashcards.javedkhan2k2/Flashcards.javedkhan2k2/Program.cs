using System.Configuration;
using Flashcards;

string? connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

if (connectionString == null)
{
    Console.WriteLine("No Connection String found.");
    return;
}

var actionManager = new ActionManager(connectionString);
actionManager.RunApp();
