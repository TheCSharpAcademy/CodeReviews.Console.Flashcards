using System.Configuration;
using Flashcards.ConsoleApp.Extensions;
using Flashcards.ConsoleApp.Views;
using Flashcards.Controllers;
using Spectre.Console;

namespace Flashcards.ConsoleApp;

/// <summary>
/// Main insertion point for the console application.
/// Configures the required application settings and launches the main menu view.
/// </summary>
internal class Program
{
    #region Methods

    private static void Main()
    {
        try
        {
            // Read required configuration settings.
            string databaseConnectionString = ConfigurationManager.AppSettings.GetString("DatabaseConnectionString");
            bool seedDatabase = ConfigurationManager.AppSettings.GetBoolean("SeedDatabase");

            // Create the required services.
            var flashcardController = new FlashcardController(databaseConnectionString);
            var stackController = new StackController(databaseConnectionString);
            var studySessionController = new StudySessionController(databaseConnectionString);
            var studySessionReportController = new StudySessionReportController(databaseConnectionString);
            var seedController = new SeedController(flashcardController, stackController, studySessionController);

            // Generate seed data if required (config value set and empty database).
            if (seedDatabase && stackController.GetStacks().Count == 0)
            {
                // Could be a long(ish) process, so show a spinner while it works.
                AnsiConsole.Status()
                    .Spinner(Spinner.Known.Aesthetic)
                    .Start("Generating seed data. Please wait...", ctx =>
                    {
                        seedController.SeedDatabase();
                        
                    });
                AnsiConsole.WriteLine("Seed data generated.");
            }

            // Show the main menu.
            var mainMenu = new MainMenuPage(flashcardController, stackController, studySessionController, studySessionReportController);
            mainMenu.Show();
        }
        catch (Exception exception)
        {
            MessagePage.Show("Error", exception);
        }
        finally
        {
            Environment.Exit(0);
        }
    }

    #endregion
}
