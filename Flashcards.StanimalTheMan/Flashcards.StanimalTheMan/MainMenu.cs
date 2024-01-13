using Spectre.Console;

namespace Flashcards.StanimalTheMan;

internal class MainMenu
{
    internal static void ShowMenu()
    {
        string[] menuOptions = new string[] { "exit", "Manage Stacks", "Manage FlashCards", "Study", "view study session data" };

        bool closeMenu = false;
        
        while (closeMenu == false)
        {
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("-----------------")
                .PageSize(5)
                .AddChoices(menuOptions));

            switch (selection)
            {
                case "exit":
                    closeMenu = true;
                    break;
                case "Manage Stacks":
                    StacksInterface.ShowMenu();
                    break;
                case "Manage FlashCards":
                    FlashcardsInterface.ShowMenu();
                    break;
                case "Study":
                    StudyInterface.ShowMenu();
                    break;
                case "view study session data":
                    StudyInterface.ShowViewMenu();
                    break;
            }
        }

        Environment.Exit(0);
    }
}
