using System.Threading.Tasks;
using Spectre.Console;

class MainMenuController
{
    public static async Task Start()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            DataBaseManager.Start();

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
                    break;
            }

            AnsiConsole.Markup("[bold green]Press Enter to continue. [/]");
            Console.Read();
        }
            
    }
}