using Flashcards.kalsson.DTOs;
using Flashcards.kalsson.Models;
using Flashcards.kalsson.Services;
using Spectre.Console;

namespace Flashcards.kalsson.UI;

public class FlashcardUI
{
    private readonly FlashcardService _flashcardService;
    private readonly StackService _stackService;

    public FlashcardUI(FlashcardService flashcardService, StackService stackService)
    {
        _flashcardService = flashcardService;
        _stackService = stackService;
    }

    public void ShowAllFlashcards()
    {
        var stackName = AnsiConsole.Ask<string>("Enter stack name:");
        var stack = _stackService.GetAllStacks().FirstOrDefault(s => s.Name == stackName);
        if (stack == null)
        {
            AnsiConsole.MarkupLine("[red]Stack not found.[/]");
            return;
        }

        var flashcards = _flashcardService.GetAllFlashcards(stack.Id).Select((f, index) => new FlashcardDTO
        {
            Id = index + 1,
            Question = f.Question,
            Answer = f.Answer
        });

        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Question");
        table.AddColumn("Answer");

        foreach (var flashcard in flashcards)
        {
            table.AddRow(flashcard.Id.ToString(), flashcard.Question, flashcard.Answer);
        }

        AnsiConsole.Write(table);
    }

    public void AddFlashcard()
    {
        var stackName = AnsiConsole.Ask<string>("Enter stack name:");
        var stack = _stackService.GetAllStacks().FirstOrDefault(s => s.Name == stackName);
        if (stack == null)
        {
            AnsiConsole.MarkupLine("[red]Stack not found.[/]");
            return;
        }

        var question = AnsiConsole.Ask<string>("Enter question:");
        var answer = AnsiConsole.Ask<string>("Enter answer:");
        var flashcard = new Flashcard { StackId = stack.Id, Question = question, Answer = answer };
        _flashcardService.AddFlashcard(flashcard);
    }

    public void UpdateFlashcard()
    {
        var id = AnsiConsole.Ask<int>("Enter flashcard ID to update:");
        var question = AnsiConsole.Ask<string>("Enter new question:");
        var answer = AnsiConsole.Ask<string>("Enter new answer:");
        var flashcard = new Flashcard { Id = id, Question = question, Answer = answer };
        _flashcardService.UpdateFlashcard(flashcard);
    }

    public void DeleteFlashcard()
    {
        var id = AnsiConsole.Ask<int>("Enter flashcard ID to delete:");
        _flashcardService.DeleteFlashcard(id);
    }
}