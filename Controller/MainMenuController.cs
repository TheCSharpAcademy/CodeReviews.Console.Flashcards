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
                    exit = true;

                    Console.Clear();
                    AnsiConsole.Write(
                        new FigletText("Goodbye! \n:)")
                            .Centered()
                            .Color(Color.Red)
                    );
                    break;
            }

            if (!exit)
            {
                AnsiConsole.Markup("[bold green]Press Enter to continue. [/]");
                Console.Read();
            }
        }
    }
    static async Task BuildTables()
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
            "Id INTEGER PRIMARY KEY",
            "Front TEXT",
            "Back TEXT"
        ]);
    }

}