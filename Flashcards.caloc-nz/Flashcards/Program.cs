using Flashcards.Config;
using Flashcards.Data;
using Flashcards.UI;
using Spectre.Console;

namespace Flashcards;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // Initialize database configuration and apply migrations
            var dbConfig = SetupDatabase();

            // Proceed to main menu
            MainMenu.Show(dbConfig);
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors by displaying a message and exiting
            AnsiConsole.MarkupLine($"[red]An unexpected error occurred: {ex.Message}[/]");
            Environment.Exit(1); // Exit the application on critical error
        }
    }

    /// <summary>
    ///     Sets up the database configuration by loading settings,
    ///     testing the connection, and reconfiguring if necessary.
    /// </summary>
    private static DatabaseConfig SetupDatabase()
    {
        var dbConfig = DatabaseConfig.LoadDatabaseConfig() ?? new DatabaseConfig();

        // Check if configuration exists, prompt user to configure if missing
        if (DatabaseConfig.LoadDatabaseConfig() == null)
        {
            AnsiConsole.MarkupLine(
                "[yellow]No database configuration found. Please configure the database settings.[/]");
            dbConfig.ConfigureDatabaseConnection();
        }

        // Test and configure the database connection
        if (!TestAndConfigureDatabase(dbConfig))
        {
            AnsiConsole.MarkupLine("[yellow]Reconfiguring database settings due to connection failure...[/]");
            dbConfig.ConfigureDatabaseConnection();

            // Retest after reconfiguration
            if (!TestAndConfigureDatabase(dbConfig))
            {
                AnsiConsole.MarkupLine("[red]Failed to connect after reconfiguration. Exiting application.[/]");
                Environment.Exit(1);
            }
        }

        AnsiConsole.MarkupLine("[green]Successfully connected to the database.[/]");
        return dbConfig;
    }

    /// <summary>
    ///     Tests the database connection and applies migrations if successful.
    /// </summary>
    private static bool TestAndConfigureDatabase(DatabaseConfig dbConfig)
    {
        using var dbContext = new AppDbContext(dbConfig);

        // Test the database connection
        if (!dbContext.TestConnection()) return false;

        // Ensure creation and apply migrations if the connection is successful
        try
        {
            dbContext.EnsureDatabaseAndMigrate();
            AnsiConsole.MarkupLine("[green]Database is up-to-date.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Failed to ensure database creation and apply migrations: {ex.Message}[/]");
            Environment.Exit(1);
        }

        return true;
    }
}