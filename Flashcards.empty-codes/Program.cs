using Spectre.Console;
using Flashcards.empty_codes.Data;
using Flashcards.empty_codes.Views;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();

string? connectionString = config.GetConnectionString("FlashcardsDb");

if (string.IsNullOrEmpty(connectionString))
{
    AnsiConsole.MarkupLine("[red]Error: Missing configuration settings.[/]");
    return;
}

var database = new Database(connectionString);
database.InitializeDatabase();

var user = new MainMenu();
user.GetMainMenu();
