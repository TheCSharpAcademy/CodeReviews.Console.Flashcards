namespace Flashcards;

class Program
{
    static void Main(string[] args)
    {
        var config = new Configuration();

        Logger.Info("Application started.");
        DatabaseSetup.CreateTablesIfNotPresent(config.DatabaseConnectionString);

        var database = new Database(config.DatabaseConnectionString);
        
        var mainMenuController = new MainMenuController();
        
        var stackController = new StackController(database);
        stackController.SetMainMenuController(mainMenuController);
        mainMenuController.SetStackController(stackController);

        var flashcardController = new FlashcardController(database);
        flashcardController.SetMainMenuController(mainMenuController);
        mainMenuController.SetFlashcardController(flashcardController);
        
        mainMenuController.ShowMainMenu();
    }
}