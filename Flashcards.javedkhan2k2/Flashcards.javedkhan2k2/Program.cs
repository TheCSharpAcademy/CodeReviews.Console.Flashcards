using Flashcards;

string? connectionString =  Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
//System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");

if (connectionString == null)
{
    Console.WriteLine("No Connection String found.");
    return;
}

var actionManager = new ActionManager(connectionString);
actionManager.RunApp();
