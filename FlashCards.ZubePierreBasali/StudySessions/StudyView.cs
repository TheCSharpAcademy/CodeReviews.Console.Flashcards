using FlashCards.FlashCardsManager;
using FlashCards.FlashCardsManager.Models;
using FlashCards.FlashCardsManager.Controllers;
using Spectre.Console;
using FlashCards.Database;

namespace FlashCards.StudySessions
{
    internal class StudyView
    {
        StudyController controller;
        public StudyView(DataTools dataTools,FlashCardsController flashCardsController,StacksController stacksController,StudyController studyController)
        {
            bool run = true;
            SelectionPrompt<string> prompt;
            do
            {
                AnsiConsole.Clear();
                prompt = new();
                string option = AnsiConsole.Prompt(prompt.AddChoices("[red]Cancel[/]", "New Study Session", "Options", "Reports"));
                prompt = new();
                switch (option)
                {
                    case "New Study Session":
                        Stacks stack = flashCardsController.GetStack(dataTools);
                        studyController.NewStudySession(stack,dataTools, stacksController);
                        break;
                    case "Options":
                        option = AnsiConsole.Prompt(prompt
                            .Title("Select an Option to modify")
                            .AddChoices("[red]Cancel[/]","Number/Mode of Questions","Timer"));
                        studyController.SetStudyOptions(option);
                        break;
                    case "Reports":
                        option = AnsiConsole.Prompt(prompt.AddChoices("[red]Cancel[/]","All Stacks","One Stack"));
                        studyController.PrintReports(option, dataTools, stacksController);
                        break;
                }
                run = UserInputs.Validation("Do you want to continue with Study Sessions?");
            } while (run);
        }
    }
}
