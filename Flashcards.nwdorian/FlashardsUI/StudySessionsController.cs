using FlashardsUI.Helpers;
using FlashcardsLibrary.Models;
using FlashcardsLibrary.Repositories;
using Spectre.Console;

namespace FlashardsUI;
internal class StudySessionsController
{
    private readonly IStudySessionsRepository _studySessionsRepository;
    private readonly IStacksRepository _stacksRepository;
    private readonly IFlashCardsRepository _flashCardsRepository;
    public StudySessionsController(IStudySessionsRepository studySessionsRepository, IStacksRepository stacksRepository, IFlashCardsRepository flashcardsRepository)
    {
        _studySessionsRepository = studySessionsRepository;
        _stacksRepository = stacksRepository;
        _flashCardsRepository = flashcardsRepository;
    }
    internal async Task GetAll()
    {
        var studySessions = await _studySessionsRepository.GetAllAsync();

        List<StudySessionDTO> studySessionDTOs = new();

        foreach (var s in studySessions)
        {
            studySessionDTOs.Add(s.ToStudySessionDTO());
        }

        TableVisualization.ShowStudySessionsTable(studySessionDTOs);

        AnsiConsole.Write("Press any key to continue...");
        Console.ReadKey();
    }

    internal async Task Post()
    {
        var stack = await GetStack("Select a stack to study: ");

        if (stack.Id == 0)
        {
            return;
        }

        var flashcards = await _flashCardsRepository.GetAllAsync(stack);

        if(!flashcards.Any())
        {
            AnsiConsole.Markup($"The stack [red]{stack.Name}[/] doesn't contain any flashcards. Press any key to continue...");
            Console.ReadKey();
            return;
        }

        int score = 0;
        int count = 1;

        foreach (var flashcard in flashcards)
        {
            Console.Clear();
            AnsiConsole.WriteLine($"Answer questions from {stack.Name} stack");
            AnsiConsole.WriteLine($"Question ({count}/{flashcards.Count()}): {flashcard.Question}");

            string answer = UserInput.StringPrompt("Enter answer: ");

            if (answer.ToLower().Trim() == flashcard.Answer.ToLower().Trim())
            {
                score++;
                AnsiConsole.Write("Your answer was correct! Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                AnsiConsole.MarkupLine($"Your answer [red]{answer}[/] was incorrect!");
                AnsiConsole.Markup($"Correct answer is [green]{flashcard.Answer}[/]. Press any key to continue...");
                Console.ReadKey();
            }
            count++;
        }
        var date = DateTime.Now;

        await _studySessionsRepository.AddAsync(new StudySession
        {
            StackId = stack.Id,
            Date = date,
            Score = score
        });

        Console.Clear();
        AnsiConsole.WriteLine($"You have finished the study session with a total score of {score}/{flashcards.Count()}!");
        AnsiConsole.Write("Press any key to continue...");
        Console.ReadLine();
    }

    internal async Task<Stack> GetStack(string prompt)
    {
        IEnumerable<Stack> stacks = await _stacksRepository.GetAllAsync();

        return AnsiConsole.Prompt(
            new SelectionPrompt<Stack>()
            .Title(prompt)
            .AddChoices(stacks)
            .AddChoices(new Stack { Id = 0, Name = "Cancel and return to menu" })
            .UseConverter(stack => stack.Name!)
            );
    }
}
