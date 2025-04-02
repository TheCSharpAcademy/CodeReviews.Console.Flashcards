using Spectre.Console;


class MainMenuController
{
    public static async Task Start()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();

            AnsiConsole.Write(
                new FigletText("Flashcards Program")
                    .Centered()
                    .Color(Color.Blue)
            );
            await BuildTables();

            exit = await HandleUserInput();

            if (!exit)
            {
                AnsiConsole.Markup("[bold green]Press Enter to continue. [/]");
                Console.Read();
            }
        }
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
    }

    private static async Task<bool> HandleUserInput()
    {
        Enums.MainMenuOptions userInput = DisplayMenu.MainMenu();
        switch (userInput)
        {
            case Enums.MainMenuOptions.MANAGESTACKS:
                await ManageStacksMenuController.Start();
                break;
            case Enums.MainMenuOptions.MANAGEFLASHCARDS:
                await ManageFlashCardsMenuController.Start();
                break;
            case Enums.MainMenuOptions.STUDY:
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
}