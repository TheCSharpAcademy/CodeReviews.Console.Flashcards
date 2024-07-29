
using Flashcards.Models;
using Flashcards.Repositories;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Flashcards.Handlers.Stack;
public class AddFlashcardHandler : IStackActionHandler {
    private readonly IStackRepository _stackRepository;

    public AddFlashcardHandler(IStackRepository stackRepository) {
        _stackRepository = stackRepository;
    }

    public async Task<bool> HandleAsync(Models.Stack stack) {
        AnsiConsole.MarkupLine($"Adding a flashcard to stack: [aqua]{stack.Name}[/]");

        var flashcard = new Flashcard {
            Question = AnsiConsole.Ask<string>("What's the question?"),
            Answer = AnsiConsole.Ask<string>("What's the answer?")
        };

        // Check for duplicate flashcard
        if (stack.Flashcards.Any(fc => fc.Question == flashcard.Question)) {
            AnsiConsole.MarkupLine("[red]A flashcard with the same question already exists in the stack.[/]");
            return false;
        }

        if (AnsiConsole.Confirm("Are you sure you want to add the flashcard?")) {
            try {
                stack.Flashcards.Add(flashcard);
                await _stackRepository.EditAsync(stack);

                AnsiConsole.MarkupLine("[green]Flashcard successfully added![/]\n");
            } catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true) {
                AnsiConsole.MarkupLine("[red]A flashcard with the same question already exists in the database.[/]");
                return false;
            } catch (Exception ex) {
                AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
                return false;
            }
        } else {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]\n");
        }

        return true;
    }

}
