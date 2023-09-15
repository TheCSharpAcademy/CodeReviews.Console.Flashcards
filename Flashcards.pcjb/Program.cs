namespace Flashcards;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Flashcards starts. Please wait a moment...");
        Logger.Info("Application started.");

        var config = new Configuration();

        if (!DatabaseSetup.CreateTablesIfNotPresent(config.DatabaseConnectionString))
        {
            Console.WriteLine("Database setup failed. Please check Flashcards.log for error details.");
            Environment.Exit(1);
        }

        var database = new Database(config.DatabaseConnectionString);
        
        var mainMenuController = new MainMenuController();
        
        var stackController = new StackController(database);
        stackController.SetMainMenuController(mainMenuController);
        mainMenuController.SetStackController(stackController);

        var flashcardController = new FlashcardController(database);
        flashcardController.SetMainMenuController(mainMenuController);
        mainMenuController.SetFlashcardController(flashcardController);

        var studySessionController = new StudySessionController(database);
        studySessionController.SetMainMenuController(mainMenuController);
        mainMenuController.SetStudySessionController(studySessionController);
        
        mainMenuController.ShowMainMenu();
    }
}