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

        var flashcards = _flashcardService.GetAllFlashcards(stack.Id).Select(f => new FlashcardDTO
        {
            Id = f.Id,
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
        var stackName = AnsiConsole.Ask<string>("Enter stack name (type 'back' to return to menu):");

        if (stackName.ToLower() == "back")
        {
            return;
        }

        var stack = _stackService.GetAllStacks().FirstOrDefault(s => s.Name == stackName);
        if (stack == null)
        {
            AnsiConsole.MarkupLine("[red]Stack not found.[/]");
            return;
        }

        var question = AnsiConsole.Ask<string>("Enter question (type 'back' to return to menu):");

        if (question.ToLower() == "back")
        {
            return;
        }

        var answer = AnsiConsole.Ask<string>("Enter answer (type 'back' to return to menu):");

        if (answer.ToLower() == "back")
        {
            return;
        }

        try
        {
            var flashcard = new Flashcard { StackId = stack.Id, Question = question, Answer = answer };
            _flashcardService.AddFlashcard(flashcard);
            AnsiConsole.MarkupLine("[green]Flashcard has been successfully added![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Failed to add the flashcard: {ex.Message}[/]");
        }
    }

    public void UpdateFlashcard()
    {
        var idInput = AnsiConsole.Ask<string>("Enter flashcard ID to update (type 'back' to return to menu):");

        if (idInput.ToLower() == "back")
        {
            return;
        }

        if (!Int32.TryParse(idInput, out var id))
        {
            AnsiConsole.MarkupLine($"[red]Invalid input: {idInput}. Please enter a valid ID.[/]");
            return;
        }

        // Check if the Flashcard with this ID exists
        var flashcard = _flashcardService.GetFlashcardById(id);
        if (flashcard == null)
        {
            AnsiConsole.MarkupLine($"[red]Flashcard with ID {id} does not exist.[/]");
            return;
        }

        var question = AnsiConsole.Ask<string>("Enter new question (type 'back' to return to menu):");

        if (question.ToLower() == "back")
        {
            return;
        }

        var answer = AnsiConsole.Ask<string>("Enter new answer (type 'back' to return to menu):");

        if (answer.ToLower() == "back")
        {
            return;
        }

        flashcard.Question = question;
        flashcard.Answer = answer;

        try
        {
            _flashcardService.UpdateFlashcard(flashcard);
            Console.Clear();
            AnsiConsole.MarkupLine("[green]Flashcard has been successfully updated![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Failed to update the flashcard: {ex.Message}[/]");
        }
    }
    public void DeleteFlashcard()
    {
        var stackName = AnsiConsole.Ask<string>("Enter stack name (type 'back' to return to menu):");

        if (stackName.ToLower() == "back")
        {
            return;
        }

        var stack = _stackService.GetAllStacks().FirstOrDefault(s => s.Name == stackName);
        if (stack == null)
        {
            AnsiConsole.MarkupLine("[red]Stack not found.[/]");
            return;
        }

        var flashcards = _flashcardService.GetAllFlashcards(stack.Id).ToList();
        if (!flashcards.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No flashcards found in this stack.[/]");
            return;
        }

        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Question");
        table.AddColumn("Answer");

        foreach (var flashcard in flashcards)
        {
            table.AddRow(flashcard.Id.ToString(), flashcard.Question, flashcard.Answer);
        }

        AnsiConsole.Write(table);

        while (true)
        {
            var idInput = AnsiConsole.Ask<string>("Enter flashcard ID to delete (type 'back' to return to menu):");

            if (idInput.ToLower() == "back")
            {
                return;
            }

            if (!int.TryParse(idInput, out var id))
            {
                AnsiConsole.MarkupLine($"[red]Invalid input: {idInput}. Please enter a valid numeric ID.[/]");
                continue;
            }

            var flashcard = flashcards.FirstOrDefault(f => f.Id == id);
            if (flashcard == null)
            {
                AnsiConsole.MarkupLine($"[red]Flashcard with ID {id} does not exist in the specified stack.[/]");
                continue;
            }

            try
            {
                _flashcardService.DeleteFlashcard(id);
                Console.Clear();
                AnsiConsole.MarkupLine("[green]Flashcard has been successfully deleted![/]");
                break;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Failed to delete the flashcard: {ex.Message}[/]");
            }
        }
    }

}