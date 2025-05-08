using Flashcards.DAL;
using Flashcards.DAL.DTO;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace Flashcards.UserInput;

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
        AnsiConsole.MarkupLine("[italic hotpink3_1 on black]3) Get number of Study Sessions per month per stack[/]");
        AnsiConsole.MarkupLine("[italic hotpink3_1 on black]4) Get average score of Study Sessions per month per stack[/]");

        string input = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[italic hotpink3_1 on black]Please type one of the following values only:[/]")
            .AddChoices([
                    "0",
                    "1",
                    "2",
                    "3",
                    "4"
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
            case "3":
                GetStudySessionsPerMonthPerStack();
                break;
            case "4":
                GetAverageScoreOfStudySessionsPerMonthPerStack();
                break;
            default:
                Console.WriteLine("Invalid command!");
                break;
        }
    }

    private void StudyStack()
    {
        string name = _validation.GetExistingStackName("[darkcyan]Please enter the name of the stack you would like to study[/]");
        List<FlashcardStackDto> stack = _controller.GetStackByName(name);
        int score = 0;
        if (!stack.IsNullOrEmpty())
        {
            AnsiConsole.MarkupLine($"[white on green]{stack[0].StackName}:[/]");
            foreach (FlashcardStackDto flashcard in stack)
            {
                AnsiConsole.MarkupLine($"[white on green]Flashcard {flashcard.ID}: \n Front text (question): {flashcard.Front}[/]");
                string answer = AnsiConsole.Ask<string>("[darkcyan]What's the answer?[/]");
                if (answer == flashcard.Back)
                {
                    AnsiConsole.MarkupLine("[lightcyan1 on mistyrose3]Correct![/]");
                    score++;
                }
                else
                {
                    AnsiConsole.MarkupLine($"[black on darkred_1]Incorrect, the answer was {flashcard.Back}[/]");
                }
            }
        }

        if (_controller.CreateStudySession(DateTime.Now, score, name))
            AnsiConsole.MarkupLine($"[white on green]Study Session finished and saved. Your final score was {score}[/]");
        else
            AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to save study session![/]");
    }

    private void GetAllStudySessions()
    {
        List<StudySessionDto> studySessions = _controller.GetAllStudySessions();
        if (!studySessions.IsNullOrEmpty())
        {
            foreach (StudySessionDto studySession in studySessions)
            {
                AnsiConsole.MarkupLine($"[white on green]Study session with ID of {studySession.ID} with score of {studySession.Score} took place at {studySession.Date.ToString("dd-MM-yyyy")} and belongs to stack '{studySession.StackName}'[/]");
            }
        }
        else
            AnsiConsole.MarkupLine($"[white on red]No study sessions found![/]");
    }

    private void GetStudySessionsPerMonthPerStack()
    {
        int year = _validation.GetValidYear();
        List<StudySessionPerMonthDto> studySessions = _controller.GetStudySessionsPerMonthPerStack(year);
        if (!studySessions.IsNullOrEmpty())
        {
            AnsiConsole.MarkupLine($"[green]Number of Study Sessions per month for year {year}[/]");
            AnsiConsole.MarkupLine($@"[green]| {nameof(StudySessionPerMonthDto.StackName)} | {nameof(StudySessionPerMonthDto.January)} | {nameof(StudySessionPerMonthDto.February)} | {nameof(StudySessionPerMonthDto.March)} | {nameof(StudySessionPerMonthDto.April)} | {nameof(StudySessionPerMonthDto.May)} | {nameof(StudySessionPerMonthDto.June)} | {nameof(StudySessionPerMonthDto.July)} | {nameof(StudySessionPerMonthDto.August)} | {nameof(StudySessionPerMonthDto.September)} | {nameof(StudySessionPerMonthDto.October)} | {nameof(StudySessionPerMonthDto.November)} | {nameof(StudySessionPerMonthDto.December)}[/]");
            AnsiConsole.MarkupLine("-------------------------------------------------------------------------------------------------------------");
            foreach (StudySessionPerMonthDto studySession in studySessions)
            {
                AnsiConsole.MarkupLine($@"[green]| {studySession.StackName} | {studySession.January} | {studySession.February} | {studySession.March} | {studySession.April} | {studySession.May} | {studySession.June} | {studySession.July} | {studySession.August} | {studySession.September} | {studySession.October} | {studySession.November} | {studySession.December} |[/]");
                AnsiConsole.MarkupLine("-------------------------------------------------------------------------------------------------------------");
            }
        }
        else
            AnsiConsole.MarkupLine($"[white on red]No study sessions found![/]");
    }

    private void GetAverageScoreOfStudySessionsPerMonthPerStack()
    {
        int year = _validation.GetValidYear();
        List<StudySessionPerMonthDto> studySessions = _controller.GetAverageScoreOfStudySessionsPerMonthPerStack(year);
        if (!studySessions.IsNullOrEmpty())
        {
            AnsiConsole.MarkupLine($"[green]Average Score of Study Sessions per month for year {year}[/]");
            AnsiConsole.MarkupLine($@"[green]| {nameof(StudySessionPerMonthDto.StackName)} | {nameof(StudySessionPerMonthDto.January)} | {nameof(StudySessionPerMonthDto.February)} | {nameof(StudySessionPerMonthDto.March)} | {nameof(StudySessionPerMonthDto.April)} | {nameof(StudySessionPerMonthDto.May)} | {nameof(StudySessionPerMonthDto.June)} | {nameof(StudySessionPerMonthDto.July)} | {nameof(StudySessionPerMonthDto.August)} | {nameof(StudySessionPerMonthDto.September)} | {nameof(StudySessionPerMonthDto.October)} | {nameof(StudySessionPerMonthDto.November)} | {nameof(StudySessionPerMonthDto.December)}[/]");
            AnsiConsole.MarkupLine("[green]------------------------------------------------------------------------------------------------------------------[/]");
            foreach (StudySessionPerMonthDto studySession in studySessions)
            {
                AnsiConsole.MarkupLine($@"[green]| {studySession.StackName} | {studySession.January} | {studySession.February} | {studySession.March} | {studySession.April} | {studySession.May} | {studySession.June} | {studySession.July} | {studySession.August} | {studySession.September} | {studySession.October} | {studySession.November} | {studySession.December} |[/]");
                AnsiConsole.MarkupLine("[green]--------------------------------------------------------------------------------------------------------------[/]");
            }
        }
        else
            AnsiConsole.MarkupLine($"[white on red]No study sessions found![/]");
    }
}
