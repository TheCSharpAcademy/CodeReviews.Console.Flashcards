using Flashcards.Config;
using Flashcards.Data;
using Spectre.Console;

namespace Flashcards.UI;

public class MainMenu
{
    public static void Show(DatabaseConfig config)
    {
        using var dbContext = new AppDbContext(config);

        var isRunning = true;

        while (isRunning)
            try
            {
                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Choose an option")
                        .AddChoices("Study", "Report", "Options", "Exit"));

                switch (selection)
                {
                    case "Study":
                        StudyMenu.Show(dbContext);
                        break;

                    case "Report":
                        ReportMenu.Show(dbContext);
                        break;

                    case "Options":
                        OptionsMenu.Show(config);
                        break;

                    case "Exit":
                        isRunning = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]An unexpected error occurred: {ex.Message}[/]");
            }
    }
}