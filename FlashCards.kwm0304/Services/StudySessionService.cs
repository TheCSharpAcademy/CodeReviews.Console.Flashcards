using System.Text.RegularExpressions;
using FlashCards.kwm0304.Models;
using FlashCards.kwm0304.Repositories;
using FlashCards.kwm0304.Views;
using Spectre.Console;

namespace FlashCards.kwm0304.Services;

public class StudySessionService
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

    public Task CreateStudySessionAsync(int stackId, int score)
    {
        throw new NotImplementedException();
    }

    public Task<List<StudySession>> GetAllStudySessionsAsync()
    {
        throw new NotImplementedException();
    }
    public async Task HandleStudy()
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
        string[] answerArr = Regex.Replace(answer, @"[^\w\s]", "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string[] correctArr = Regex.Replace(correctAnswer, @"[^\w\s]", "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
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
}