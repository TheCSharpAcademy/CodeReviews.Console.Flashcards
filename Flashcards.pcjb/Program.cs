namespace Flashcards;

class Program
{
    static void Main(string[] args)
    {
        var config = new Configuration();

        Logger.Info("Application started.");
        DatabaseSetup.CreateTablesIfNotPresent(config.DatabaseConnectionString);

        var mainMenuController = new MainMenuController();
        mainMenuController.ShowMainMenu();
    }
}