using Spectre.Console;


class MainMenuController : MenuController
{
    protected override async Task MainAsync()
    {
        DataBaseManager.Start();
        AnsiConsole.Write(
            new FigletText("Flashcards Program")
                .Centered()
                .Color(Color.Blue)
        );
        await BuildTables();
    }

    protected override async Task<bool> HandleMenuAsync()
    {
        Enums.MainMenuOptions userInput = DisplayMenu.MainMenu();
        switch (userInput)
        {
            case Enums.MainMenuOptions.MANAGESTACKS:
                StacksMenuController stacksMenuController = new();
                await stacksMenuController.StartAsync();
                break;
            case Enums.MainMenuOptions.MANAGEFLASHCARDS:
                FlashCardsMenuController flashCardsMenuController = new();
                await flashCardsMenuController.StartAsync();
                break;
            case Enums.MainMenuOptions.STUDY:
                StudyController studyController = new();
                await studyController.StartAsync();
                break;
            case Enums.MainMenuOptions.VIEWSTUDYDATA:
                ViewStudyDataController viewStudyDataController = new();
                await viewStudyDataController.StartAsync();
                break;
            case Enums.MainMenuOptions.EXIT:
                Console.Clear();
                AnsiConsole.Write(
                    new FigletText("Goodbye! \n:)")
                        .Centered()
                        .Color(Color.Red)
                );
                return true;
        }
        return false;
    }
    public static async Task BuildTables() // public just for debug purposes
    {
        StacksDatabaseManager stacksDatabaseManager = new();
        await stacksDatabaseManager.BuildTable();

        FlashcardsDatabaseManager flashcardsDatabaseManager = new();
        await flashcardsDatabaseManager.BuildTable();

        StudySessionDatabaseManager studySessionDatabaseManager = new();
        await studySessionDatabaseManager.BuildTable();
    }
}