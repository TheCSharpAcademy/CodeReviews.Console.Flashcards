using Spectre.Console;

class ManageFlashCardsMenuController
{
    public static async Task Start()
    {
        Console.Clear();

        Enums.ManageFlashCardsMenuOptions userInput = DisplayMenu.ManageFlashCardsMenu();

        switch (userInput)
        {
            case Enums.ManageFlashCardsMenuOptions.VIEWALLCARDS:
                break;
            case Enums.ManageFlashCardsMenuOptions.VIEWXCARDS:
                break;
            case Enums.ManageFlashCardsMenuOptions.CREATECARD:
                break;
            case Enums.ManageFlashCardsMenuOptions.EDITCARD:
                break;
            case Enums.ManageFlashCardsMenuOptions.DELETECARD:
                break;
            case Enums.ManageFlashCardsMenuOptions.BACK:
                break;
        }
    }
}