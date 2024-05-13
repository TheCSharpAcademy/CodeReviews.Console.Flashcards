using FlashardsUI.Helpers;
using FlashcardsLibrary.Models;
using FlashcardsLibrary.Repositories;
using Spectre.Console;

namespace FlashardsUI;
internal class FlashcardsController
{
    private readonly IFlashCardsRepository _flashcardsRepository;
    public FlashcardsController(IFlashCardsRepository flashcardsRepository)
    {
        _flashcardsRepository = flashcardsRepository;
    }
    public Stack? CurrentStack { get; set; }

    internal async Task GetAll()
    {
        var flashcards = await _flashcardsRepository.GetAllAsync(CurrentStack);

        List<FlashcardDTO> flashcardDTOs = new();

        foreach (var flashcard in flashcards)
        {
            flashcardDTOs.Add(flashcard.ToFlashcardDTO());
        }

        TableVisualization.ShowFlashcardsTable(flashcardDTOs, CurrentStack.Name);

        AnsiConsole.Write("Press any key to continue... ");
        Console.ReadKey();
    }

    internal async Task Post()
    {
        AnsiConsole.MarkupLine($"Add new flashcard to the [blue]{CurrentStack.Name}[/] stack");

        var question = UserInput.StringPrompt("Enter flashcard question (or press 0 to cancel):");

        while (await _flashcardsRepository.FlashcardExistsAsync(question.Trim()))
        {
            Console.Clear();
            question = UserInput.StringPrompt($"Flashcard with the question [red]{question}[/] already exists! \n\nEnter question (or press 0 to cancel):");
        }

        if (question.Trim() == "0")
        {
            return;
        }

        var answer = UserInput.StringPrompt("Enter flashcard answer (or press 0 to cancel):");

        if (answer.Trim() == "0")
        {
            return;
        }

        if (!AnsiConsole.Confirm($"Are you sure you want to add a new flashcard to the [blue]{CurrentStack.Name}[/] stack?"))
        {
            return;
        }

        await _flashcardsRepository.AddAsync(new Flashcard
        {
            StackId = CurrentStack.Id,
            Question = question,
            Answer = answer
        });

        AnsiConsole.Write($"\nNew flashcard was succesfully added! Press any key to continue...");
        Console.ReadKey();
    }

    internal async Task Delete()
    {
        var flashcard = await Get("Select a flashcard to delete:");

        if (flashcard.Id == 0)
        {
            return;
        }

        if (!AnsiConsole.Confirm($"Are you sure you want to delete [green]{flashcard.Question}[/]?"))
        {
            return;
        }

        await _flashcardsRepository.DeleteAsync(flashcard);

        AnsiConsole.Markup($"\nFlashcard [green]{flashcard.Question}[/] was succesfully deleted! Press any key to continue...");
        Console.ReadKey();
    }

    internal async Task Update()
    {
        var flashcard = await Get("Select a flashcard to update:");

        if (flashcard.Id == 0)
        {
            return;
        }

        AnsiConsole.Markup($"Editing flashcard \nQuestion: [green]{flashcard.Question}[/]\nAnswer: [green]{flashcard.Answer}[/]\n\n");

        var question = UserInput.StringPromptAllowEmpty("Enter flashcard question (or press 0 to cancel):");

        while (await _flashcardsRepository.FlashcardExistsAsync(question.Trim()))
        {
            Console.Clear();
            AnsiConsole.Markup($"Editing flashcard \nQuestion: [green]{flashcard.Question}[/]\nAnswer: [green]{flashcard.Answer}[/]\n\n");
            question = UserInput.StringPromptAllowEmpty($"Flashcard with the question [red]{question}[/] already exists! Enter flashcard question (leave empty to skip):");
        }

        if (question.Trim() == "0")
        {
            return;
        }

        var answer = UserInput.StringPromptAllowEmpty("Enter flashcard answer (or press 0 to cancel):");

        if (answer.Trim() == "0")
        {
            return;
        }

        if (!AnsiConsole.Confirm($"Are you sure you want to apply flashcard changes?"))
        {
            return;
        }

        if (question != "")
        {
            flashcard.Question = question;
        }
        if (answer != "")
        {
            flashcard.Answer = answer;
        }

        await _flashcardsRepository.UpdateAsync(flashcard);

        AnsiConsole.Write($"\nFlashcard was succesfully updated! Press any key to continue...");
        Console.ReadKey();
    }

    internal async Task<Flashcard> Get(string prompt)
    {
        IEnumerable<Flashcard> flashcards = await _flashcardsRepository.GetAllAsync(CurrentStack);

        return AnsiConsole.Prompt(
            new SelectionPrompt<Flashcard>()
            .Title(prompt)
            .AddChoices(flashcards)
            .AddChoices(new Flashcard { Id = 0, StackId = 0, Question = "Cancel and return to menu", Answer = "" })
            .UseConverter(f => f.Question)
            );
    }
}
