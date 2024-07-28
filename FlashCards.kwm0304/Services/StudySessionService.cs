using FlashCards.kwm0304.Models;
using FlashCards.kwm0304.Repositories;
using FlashCards.kwm0304.Views;
using FlashCards.kwm0304.Utils;
using Spectre.Console;

namespace FlashCards.kwm0304.Services;

public partial class StudySessionService
{
    private readonly StudySessionRepository _repository;
    private readonly StackService _stackService;
    private readonly FlashCardService _flashcardService;
    private readonly StudySessionsTable _table;
    private readonly QuizUtils _utils;
    public StudySessionService()
    {
        _repository = new StudySessionRepository();
        _stackService = new StackService();
        _flashcardService = new FlashCardService();
        _table = new StudySessionsTable(_stackService, _repository);
        _utils = new QuizUtils();
    }

    public async Task HandleStudy()
    {
        string choice = SelectionPrompt.StudyMenu();
        if (choice == "Study")
        {
            await StudySession();
        }
        else if (choice == "View all study sessions")
        {
            await _table.AllSessionsTable();
        }
        else
        {
            return;
        }
    }

    private async Task StudySession()
    {
        Stack? stack = await ChooseTopic();
        if (stack == null)
        {
            return;
        }
        int id = stack.StackId;
        List<FlashCard> flashcards = await _flashcardService.GetShuffledCardsAsync(id);
        _utils.DisplayInstructions();
        int score = _utils.TakeQuiz(flashcards);
        await _repository.CreateSessionAsync(score, id);
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