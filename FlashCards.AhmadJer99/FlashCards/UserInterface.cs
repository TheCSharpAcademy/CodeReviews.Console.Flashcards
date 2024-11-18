using FlashCards.Managers;
using Spectre.Console;
using static FlashCards.MenuEnums;

namespace FlashCards;

internal class UserInterface
{
    internal static void ShowMainMenu()
    {
        bool exitApp = false;

        do
        {
            Console.Clear();

            var mainMenuUserChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<MainMenuAction>()
                        .Title("[yellow]Choose an operation from the following list:[/]")
                        .AddChoices(Enum.GetValues<MainMenuAction>()));


            switch (mainMenuUserChoice)
            {
                case MainMenuAction.Study:
                    StudySessionArea studySessionArea = new StudySessionArea();
                    studySessionArea.ShowMenu();
                    break;
                case MainMenuAction.ManageStacks:
                    StacksManager stacksManager = new StacksManager();
                    stacksManager.ShowOptions();
                    break;
                case MainMenuAction.ManageFlashCards:
                    CardsManager cardsManager = new CardsManager();
                    cardsManager.ShowMenu();
                    break;
                case MainMenuAction.ViewPreviousSessionsData:
                    ViewPreviousSessionsData.ChooseReport();
                    break;
                case MainMenuAction.Exit:
                    AnsiConsole.MarkupLine("[green]Thanks for using the application\nGoodBye![/]");
                    exitApp = true;
                    break;
            }
        }
        while (!exitApp);
    }
}
