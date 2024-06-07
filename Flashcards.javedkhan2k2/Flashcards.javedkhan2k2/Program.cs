using Flashcards;

string? connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

if (connectionString == null)
{
    Console.WriteLine("No Connection String found.");
    return;
}

var actionManager = new ActionManager(connectionString);
actionManager.RunApp();
