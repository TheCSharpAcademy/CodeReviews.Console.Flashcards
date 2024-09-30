using Flashcards.empty_codes.Data;
using Flashcards.empty_codes.Models;
using Flashcards.empty_codes.Controllers;
using Spectre.Console;

namespace Flashcards.empty_codes.Views
{
    internal class MainMenu
    {
        public StacksController? stackController { get; }
        public FlashcardsController? flashcardsController { get; }
        public StudySessionController? studySessionController { get; }

        public void GetMainMenu()
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose an [green]option below[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal your choices)[/]")
                    .AddChoices(new[] {
                        "Manage Stacks",
                        "Manage Study Sessions", "Exit",
                    }));

            switch (choice)
            {
                case "Manage Stacks":
                    var stackMenu = new StackMenu();
                    stackMenu.GetStackMenu();
                    break;
                case "Manage Study Sessions":
                    var sessionMenu = new StudySessionMenu();
                    sessionMenu.GetStudySessionMenu();
                    break;
                case "Exit":
                    return;
                default:
                    AnsiConsole.WriteLine("Invalid selection. Please try again.");
                    break;
            }
        }    
    }
}