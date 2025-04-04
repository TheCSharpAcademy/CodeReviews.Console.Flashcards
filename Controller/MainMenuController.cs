using Spectre.Console;


class MainMenuController : MenuController
{
    protected override async Task MainAsync()
    {
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
                await studyController.Start();
                break;
            case Enums.MainMenuOptions.VIEWSTUDYDATA:
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
        DataBaseManager<Stack>.Start("stacks");
        await DataBaseManager<Stack>.BuildTable(
        [
            "Id INTEGER IDENTITY(1,1) PRIMARY KEY",
            "Name TEXT"
        ]);

        DataBaseManager<Flashcard>.Start("flash_cards");
        await DataBaseManager<Flashcard>.BuildTable(
        [
            "Stacks_Id INTEGER NOT NULL",
            "FOREIGN KEY (Stacks_Id) REFERENCES stacks (Id)",
            "Id INTEGER",
            "Front TEXT",
            "Back TEXT"
        ]);

        DataBaseManager<StudySession>.Start("study_sessions");
        await DataBaseManager<StudySession>.BuildTable([
            "Stacks_Id INTEGER NOT NULL",
            "FOREIGN KEY (Stacks_Id) REFERENCES stacks (Id)",
            "Date TEXT",
            "Score INTEGER"
        ]);
    }
}