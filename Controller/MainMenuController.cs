using Spectre.Console;

class MainMenuController
{
    public static void Start()
    {
        Console.Clear();
        DataBaseManager.Start();

        Enums.MainMenuOptions userInput = DisplayMenu.MainMenu();

        switch (userInput)
        {
            case Enums.MainMenuOptions.MANAGESTACKS:
                ManageStacksMenuController.Start();
                break;
            case Enums.MainMenuOptions.MANAGEFLASHCARDS:
                ManageFlashCardsMenuController.Start();
                break;
            case Enums.MainMenuOptions.STUDY:
                break;
            case Enums.MainMenuOptions.EXIT:
                break;
        }
    }
}