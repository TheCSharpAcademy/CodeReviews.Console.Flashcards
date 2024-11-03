using System.Text.Json;
using Flashcards.Helpers;
using Spectre.Console;

namespace Flashcards.Config;

public class DatabaseConfig
{
    private const string ConfigFilePath = "database_config.json";
    private const int DefaultSqlPort = 1433;
    public string ConnectionString { get; set; } = null!;

    public static DatabaseConfig? LoadDatabaseConfig()
    {
        try
        {
            if (File.Exists(ConfigFilePath))
            {
                var json = File.ReadAllText(ConfigFilePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    AnsiConsole.MarkupLine(
                        "[red]Configuration file is empty. Please reconfigure the database settings.[/]");
                    return null;
                }

                var config = JsonSerializer.Deserialize<DatabaseConfig>(json);
                if (config == null || string.IsNullOrWhiteSpace(config.ConnectionString))
                {
                    AnsiConsole.MarkupLine(
                        "[red]Invalid configuration file. Please reconfigure the database settings.[/]");
                    return null;
                }

                return config;
            }

            AnsiConsole.MarkupLine(
                "[yellow]Configuration file not found. Please configure the database settings.[/]");
        }
        catch (JsonException)
        {
            AnsiConsole.MarkupLine(
                "[red]Invalid JSON format in configuration file. Please reconfigure the database settings.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]An unexpected error occurred: {ex.Message}[/]");
        }

        return null;
    }

    public void ConfigureDatabaseConnection()
    {
        try
        {
            // Prompt for server address
            var server = ConsoleHelper.PromptWithValidation("Enter the [green]server address[/]:",
                "[red]Server address cannot be empty.[/]");
            if (string.IsNullOrWhiteSpace(server)) return;

            // Prompt for port, defaulting to 1433 if left blank
            var port = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter the [green]port[/] (leave blank for default 1433):").AllowEmpty());
            var serverWithPort = string.IsNullOrWhiteSpace(port) ? $"{server},{DefaultSqlPort}" : $"{server},{port}";

            // Prompt for database name
            var database = ConsoleHelper.PromptWithValidation("Enter the [green]database name[/]:",
                "[red]Database name cannot be empty.[/]");
            if (string.IsNullOrWhiteSpace(database)) return;

            // Prompt for username
            var username =
                ConsoleHelper.PromptWithValidation("Enter the [green]username[/]:",
                    "[red]Username cannot be empty.[/]");
            if (string.IsNullOrWhiteSpace(username)) return;

            // Prompt for password
            var password = AnsiConsole.Prompt(new TextPrompt<string>("Enter the [green]password[/]:").PromptStyle("red")
                .Secret());

            // Build and save the connection string
            ConnectionString =
                $"Server={serverWithPort};Database={database};User Id={username};Password={password};TrustServerCertificate=True;";

            var json = JsonSerializer.Serialize(this);
            File.WriteAllText(ConfigFilePath, json);

            AnsiConsole.MarkupLine("[green]Database connection settings saved.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]An error occurred while saving the database configuration: {ex.Message}[/]");
        }
    }
}