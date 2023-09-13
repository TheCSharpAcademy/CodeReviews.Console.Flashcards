namespace Flashcards;

class Program
{
    static void Main(string[] args)
    {
        var config = new Configuration();

        Logger.Info("Application started.");
        Console.WriteLine("Flashcards");
        DatabaseSetup.CreateTablesIfNotPresent(config.DatabaseConnectionString);
    }
}