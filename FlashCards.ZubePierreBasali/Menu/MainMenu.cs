using FlashCards.Database;
using FlashCards.FlashCardsManager.Controllers;
using FlashCards.FlashCardsManager.Views;
using FlashCards.StudySessions;
using Spectre.Console;

namespace FlashCards.Menu
{
    internal class MainMenu
    { 
        internal MainMenu()
        {
            DataTools dataTools = new();
            dataTools.Initialization();
            StacksController stacksController = new();
            FlashCardsController flashCardsController = new();
            StudyController studyController = new();
            AnsiConsole.MarkupLine("Welcome to [cyan]FlashCards[/].");
            bool run = true;
            do
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("Welcome to [cyan]FlashCards[/].");
                string option = AnsiConsole.Prompt(
                new SelectionPrompt<string>().AddChoices("Manage FlashCards", "Manage Stacks", "See All Stacks", "Study Session", "[red]Exit[/]", "(Test) Add test stacks and cards"));

                switch (option)
                {
                    case "Manage FlashCards":
                        new FlashCardsView(dataTools,stacksController,flashCardsController);
                        break;
                    case "Manage Stacks":
                        new StacksView(option,dataTools,stacksController);
                        break;
                    case "See All Stacks":
                        new StacksView(option,dataTools,stacksController);
                        break;
                    case "Study Session":
                        new StudyView(dataTools,flashCardsController,stacksController,studyController);
                        break;
                    case "[red]Exit[/]":
                        run = false;
                        AnsiConsole.MarkupLine("Thank you! See you for your next session!");
                        return;
                    case "(Test) Add test stacks and cards":
                        stacksController.CreateTestStacks(dataTools);
                        studyController.CreateStudySessionsData(dataTools);
                        break;
                }

                AnsiConsole.MarkupLine("Press any [yellow]Key[/] to continue.");
                Console.ReadKey();
            } while(run);
        }
    }
}
