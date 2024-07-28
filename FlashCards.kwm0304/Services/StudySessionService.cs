using System.Text.RegularExpressions;
using FlashCards.kwm0304.Models;
using FlashCards.kwm0304.Repositories;
using FlashCards.kwm0304.Views;
using Spectre.Console;

namespace FlashCards.kwm0304.Services;

public partial class StudySessionService
{
    private readonly StudySessionRepository _repository;
    private readonly StackService _stackService;
    private readonly FlashCardService _flashcardService;
    public StudySessionService()
    {
        _repository = new StudySessionRepository();
        _stackService = new StackService();
        _flashcardService = new FlashCardService();
    }

    public async Task<List<StudySession>> GetAllStudySessionsAsync()
    {
        return await _repository.GetAllSessionsAsync();
    }

    public async Task AllSessionsTable()
    {
        string[] cols = ["Studied At", "Stack", "Score"];
        List<StudySession> sessions = await GetAllStudySessionsAsync();
        if (sessions == null)
        {
            AnsiConsole.WriteLine("No study sessions to display");
            return;
        }
        List<int> stackIds = sessions.Select(s => s.StackId).Distinct().ToList();
        List<string> stackNames = await _stackService.GetAllStackNames(stackIds);
        Dictionary<int, string> mapIdToName = [];
        for (int i = 0; i < stackIds.Count; i++)
        {
            mapIdToName[stackIds[i]] = stackNames[i];
        }
        var table = new Table();
        table.Title("All Study Sessions");
        table.AddColumns(cols);
        foreach (var session in sessions)
        {
            table.AddRow(
                session.StudiedAt.ToString("g"),
                mapIdToName.TryGetValue(session.StackId, out var stackName) ? stackName : "Unknown",
                session.Score.ToString());
        }
        AnsiConsole.Write(table);
        Console.WriteLine("\nPress any key to return to the main menu...");
        Console.ReadKey(true);
    }
    public async Task HandleStudy()
    {
        string choice = SelectionPrompt.StudyMenu();
        if (choice == "Study")
        {
            Stack? stack = await ChooseTopic();
            if (stack == null)
            {
                return;
            }
            int id = stack.StackId;
            List<FlashCard> flashcards = await _flashcardService.GetShuffledCardsAsync(id);
            DisplayInstructions();
            int score = TakeQuiz(flashcards);
            await _repository.CreateSessionAsync(score, id);
        }
        else if (choice == "View all study sessions")
        {
            await AllSessionsTable();
        }
        else
        {
            return;
        }
    }

    private static int TakeQuiz(List<FlashCard> flashcards)
    {
        int score = 0;
        foreach (FlashCard card in flashcards)
        {
            Console.Clear();
            string answer = AskQuestion(card);
            string correctAnswer = card.Answer;
            bool isCorrect = CheckAnswer(answer, correctAnswer);
            if (isCorrect)
            {
                score++;
                AnsiConsole.MarkupLine($"[bold green]CORRECT![/] Score: {score}");
                Thread.Sleep(1500);
            }
            else
            {
                AnsiConsole.MarkupLine($"[bold red]INCORRECT[/] Score: {score}");
                AnsiConsole.WriteLine($"The correct answer was: {correctAnswer}");
                Thread.Sleep(1500);
            }
        }
        return score;
    }

    private static bool CheckAnswer(string answer, string correctAnswer)
    {
        string[] answerArr = Punctuation().Replace(answer, "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string[] correctArr = Punctuation().Replace(correctAnswer, "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var matchingSet = new HashSet<string>(correctArr, StringComparer.OrdinalIgnoreCase);
        foreach (string word in answerArr)
        {
            if (!matchingSet.Contains(word))
            {
                return false;
            }
        }
        return true;
    }

    public static void DisplayInstructions()
    {
        AnsiConsole.WriteLine("Your attempt will be correct if the correct answer contains all of the words in your attempt.");
        Thread.Sleep(2000);
    }

    public static string AskQuestion(FlashCard card)
    {
        AnsiConsole.MarkupLine("[bold blue]What is your answer[/]");
        return AnsiConsole.Ask<string>($"{card.Question}\n");
    }

    private async Task<Stack?> ChooseTopic()
    {
        List<Stack> stacks = await _stackService.GetAllStacks();
        Stack? selectedStack = SelectionPrompt.StackSelection(stacks);
        if (selectedStack == null)
        {
            AnsiConsole.WriteLine("No stacks to select");
            return null;
        }
        return selectedStack;
    }

    [GeneratedRegex(@"[^\w\s]")]
    private static partial Regex Punctuation();
}