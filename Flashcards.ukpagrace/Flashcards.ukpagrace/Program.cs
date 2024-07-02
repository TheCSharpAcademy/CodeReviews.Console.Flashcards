using Spectre.Console;
using Flashcards.ukpagrace.Controller;
namespace Flashcards.ukpagrace
{
    class Program
    {
        StackController stackController = new();
        FlashcardController flashcardController = new();
        StudySessionController studySessionController = new();
        bool exitApp;
        static void Main(string[] args)
        {
 
            Program program = new Program();
            program.flashcardController.CreateTable();
            program.stackController.CreateTable();
            program.studySessionController.CreateTable();

            while (!program.exitApp) {
                program.GetOption();
            }
        }


        void GetOption()
        {
            var input = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("choose a [green]group[/]?")
                    .PageSize(5)
                    .AddChoices(new[] {
                        "Stack", "FlashCard", "StudySession", "Exit"
                    })
            );

            switch (input.ToLower())
            {
                case "stack":
                    StackSwitch();
                    break;
                case "flashcard":
                    FlashcardSwitch();
                    break;
                case "studysession":
                    StudySessionSwitch();
                    break;
                case "exit":
                    ExitApp();
                    break;
            }
        }
        void StackSwitch()
        {
        
        var input = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("choose an [green] action[/]?")
                .PageSize(5)
                .AddChoices(new[] {
                  "Create", "Update", "Delete", "Back"
                })
        );
            switch (input.ToLower())
            {
                case "create":
                    stackController.CreateStack();
                    break;
                case "delete":
                    stackController.DeleteStack();
                    break;
                case "update":
                    stackController.UpdateStack();
                    break;
                case "back":
                    GetOption();
                    break;
                default: break;
            }
        }

        void FlashcardSwitch()
        {
            var input = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("choose an [green]action[/]?")
                    .PageSize(5)
                    .AddChoices(new[] {
                        "Create", "List", "Update", "Delete","Back"
                    })
            );
            switch (input.ToLower())
            {
                case "create":
                    flashcardController.CreateFlashcard();
                    break;
                case "delete":
                    flashcardController.DeleteFlashcard();
                    break;
                case "update":
                    flashcardController.UpdateFlashcard();
                    break;
                case "list":
                    flashcardController.ShowFlashCards();
                    break;
                case "back":
                    GetOption();
                    break;
                default: break;
            }
        }

        void ExitApp()
        {
            exitApp = true; 
        }

        void StudySessionSwitch()
        {
            var input = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("choose an [green]action[/]?")
                    .PageSize(5)
                    .AddChoices(new[] {
                        "Study", "SeeSessions", "SessionCountPerMonth", "AverageScorePerMonth",
                        "Back"
                    })
                );

            switch (input.ToLower())
            {
                case "study":
                    studySessionController.StartStudySession();
                    break;
                case "seesessions":
                    studySessionController.ListStudySession();
                    break;
                case "sessioncountpermonth":
                    studySessionController.GetNumberOfSessionPerMonth();
                    break;
                case "averagescorepermonth":
                    studySessionController.GetAverageScorePerMonth();
                    break;
                case "back":
                    GetOption();
                    break;
                default: break;
            }
        }
    }
}
