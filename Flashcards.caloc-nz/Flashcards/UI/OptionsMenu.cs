using Flashcards.Config;
using Flashcards.Data;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.UI;

public class OptionsMenu
{
    public static void Show(DatabaseConfig config)
    {
        var isRunning = true;

        while (isRunning)
            try
            {
                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Choose an option")
                        .AddChoices("Database Configuration", "Manage Stacks", "Manage Flashcards", "Import Flashcards",
                            "Export Flashcards", "Back"));

                switch (selection)
                {
                    case "Database Configuration":
                        config.ConfigureDatabaseConnection();

                        // Reinitialize the DbContext with the updated config and apply migrations
                        using (var dbContext = new AppDbContext(config))
                        {
                            dbContext.EnsureDatabaseAndMigrate();
                        }

                        AnsiConsole.MarkupLine(
                            "[green]Database configuration updated and migrations applied if needed.[/]");
                        break;

                    case "Manage Stacks":
                        StackMenu.Show(config);
                        break;

                    case "Manage Flashcards":
                        FlashcardMenu.Show(config);
                        break;

                    case "Import Flashcards":
                        DisplayCsvFormatTable();
                        var importPath = AnsiConsole.Ask<string>("Enter the file path of the CSV to import:");
                        using (var dbContext = new AppDbContext(config))
                        {
                            var flashcardService = new FlashcardService(dbContext);
                            flashcardService.ImportFlashcardsFromCsv(importPath);
                        }

                        break;

                    case "Export Flashcards":
                        var exportStackId =
                            AnsiConsole.Ask<int?>(
                                "Enter the stack ID to export (or press Enter to export all stacks):");
                        var exportPath = AnsiConsole.Ask<string>("Enter the file path to save the CSV:");
                        using (var dbContext = new AppDbContext(config))
                        {
                            var flashcardService = new FlashcardService(dbContext);
                            flashcardService.ExportFlashcardsToCsv(exportStackId, exportPath);
                        }

                        break;

                    case "Back":
                        isRunning = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]An unexpected error occurred: {ex.Message}[/]");
            }
    }

    private static void DisplayCsvFormatTable()
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Front");
        table.AddColumn("Back");
        table.AddColumn("StackId");
        table.AddRow("1", "Example Front Text", "Example Back Text", "123");

        AnsiConsole.MarkupLine("[bold yellow]CSV Format Guide:[/]");
        AnsiConsole.Write(table);
    }
}