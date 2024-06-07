using Flashcards;

string? connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");

if (connectionString == null)
{
    Console.WriteLine("No Connection String found.");
    return;
}

var actionManager = new ActionManager(connectionString);
actionManager.RunApp();
