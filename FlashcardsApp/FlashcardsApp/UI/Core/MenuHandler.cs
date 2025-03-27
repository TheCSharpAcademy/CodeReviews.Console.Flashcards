using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsApp.Services;
using FlashcardsApp.UI.Features;
using Spectre.Console;

namespace FlashcardsApp.UI.Core
{
    internal class MenuHandler
    {
        private readonly DatabaseService _databaseService;
        private readonly StackUI _stackUI;
        private readonly FlashcardUI _flashcardUI;
        private readonly StudyUI _studyUI;

        internal MenuHandler(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            _stackUI = new StackUI(databaseService);
            _flashcardUI = new FlashcardUI(databaseService);
            _studyUI = new StudyUI(databaseService);
        }

        internal void MainMenu()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("MAIN MENU")
                        .AddChoices(new[] {
                            "View All Stacks",
                            "Create New Stack",
                            "Manage Stack",
                            "Study a Stack",
                            "View Study History",
                            "Close Application"
                        }));

                switch (choice)
                {
                    case "View All Stacks":
                        _databaseService.GetAllStacks();
                        break;
                    case "Create New Stack":
                        _stackUI.CreateStack();
                        break;
                    case "Manage Stack":
                        _stackUI.StacksListMenu();
                        break;
                    case "Study a Stack":
                        _studyUI.StudyMain();
                        break;
                    case "View Study History":
                        _databaseService.GetStudyHistory();
                        break;
                    case "Close Application":
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
