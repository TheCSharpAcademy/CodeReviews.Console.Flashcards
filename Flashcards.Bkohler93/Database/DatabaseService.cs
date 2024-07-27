using System.Reflection;
using DbUp;

namespace Flashcards.Database;

public static class DatabaseService
{
    public static void MigrateUp(string connectionString)
    { 
        EnsureDatabase.For.SqlDatabase(connectionString);

        var upgrader =
            DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                // .LogToConsole()
                .Build();

        var result = upgrader.PerformUpgrade();

        if (result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Database ready");
            Console.ResetColor();
        }
    }
}
