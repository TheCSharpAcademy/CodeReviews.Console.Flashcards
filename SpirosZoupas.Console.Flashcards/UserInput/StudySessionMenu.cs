using Flashcards.DAL;
using Flashcards.DAL.DTO;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace Flashcards.UserInput
{
    public class StudySessionMenu : BaseMenu
    {
        public StudySessionMenu(Controller controller, Validation validation) : base(controller, validation)
        {
        }

        public void GetStudySessionMenu()
        {
            AnsiConsole.MarkupLine("[bold purple on black]Stack MENU[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]Please choose an action:[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]0) Back to Main Menu[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]1) Study a stack[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]2) Get all Study Sessions[/]");

            string input = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[italic hotpink3_1 on black]Please type one of the following values only:[/]")
                .AddChoices([
                        "0",
                        "1",
                        "2"
                ]));

            switch (input)
            {
                case "0":
                    break;
                case "1":
                    StudyStack();
                    break;
                case "2":
                    GetAllStudySessions();
                    break;
                default:
                    Console.WriteLine("Invalid command!");
                    break;
            }
        }

        private void StudyStack()
        {
            string name = _validation.GetExistingStackName("[darkcyan]Please enter the name of the stack you would like to study[/]");
            List<FlashcardStackDTO> stack = _controller.GetStackByName(name);
            if (!stack.IsNullOrEmpty())
            {
                AnsiConsole.MarkupLine($"[white on green]{stack[0].StackName}:[/]");
                foreach (FlashcardStackDTO flashcard in stack)
                {
                    AnsiConsole.MarkupLine($"[white on green]Flashcard {flashcard.ID}: \n Front text: {flashcard.Front} \n Back text: {flashcard.Back}[/]");
                }
            }
            AnsiConsole.MarkupLine($"[bold purple on black]Please press any button to stop studying[/]");
            Console.Read();
            int score = _validation.GetValidatedInteger("[darkcyan]Please enter the final score of your study session[/]");

            if (_controller.CreateStudySession(DateTime.Now, score, name))
                AnsiConsole.MarkupLine("[white on green]Study Session finished and saved.[/]");
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to save study session![/]");
        }

        private void GetAllStudySessions()
        {
            List<StudySessionDTO> studySessions = _controller.GetAllStudySessions();
            if (!studySessions.IsNullOrEmpty())
            {
                foreach (StudySessionDTO studySession in studySessions)
                {
                    AnsiConsole.MarkupLine($"[white on green]Study session with ID of {studySession.ID} with score of {studySession.Score} took place at {studySession.Date.ToString("dd-MM-yyyy")} and belongs to stack '{studySession.StackName}'[/]");
                }
            }
            else
                AnsiConsole.MarkupLine($"[white on red]No study sessions found![/]");
        }
    }
}
