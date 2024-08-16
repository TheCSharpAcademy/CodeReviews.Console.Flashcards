using Flashcards.Models;
using Flashcards.Repositories;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Flashcards.Handlers.Stack;
/// <summary>
/// Handles the addition of flashcards to a stack.
/// </summary>
public class AddFlashcardHandler : IStackActionHandler {
    private readonly IStackRepository _stackRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddFlashcardHandler"/> class.
    /// </summary>
    /// <param name="stackRepository">The repository to interact with stack data.</param>
    public AddFlashcardHandler(IStackRepository stackRepository) {
        _stackRepository = stackRepository;
    }

    /// <summary>
    /// Handles the asynchronous operation of adding a flashcard to the specified stack.
    /// </summary>
    /// <param name="stack">The stack to which the flashcard will be added.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(Models.Stack stack) {
        AnsiConsole.MarkupLine($"Adding a flashcard to stack: [aqua]{stack.Name}[/]");

        var flashcard = new Flashcard {
            Question = AnsiConsole.Ask<string>("What's the question?"),
            Answer = AnsiConsole.Ask<string>("What's the answer?")
        };

        // Check for duplicate flashcard
        if (stack.Flashcards.Any(fc => fc.Question == flashcard.Question)) {
            AnsiConsole.MarkupLine("[red]A flashcard with the same question already exists in the stack.[/]\n");
            return;
        }

        if (AnsiConsole.Confirm("Are you sure you want to add the flashcard?")) {
            try {
                stack.Flashcards.Add(flashcard);
                await _stackRepository.EditAsync(stack);

                AnsiConsole.MarkupLine("[green]Flashcard successfully added![/]\n");
            } catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true) {
                AnsiConsole.MarkupLine("[red]A flashcard with the same question already exists in the database.[/]\n");
            } catch (Exception ex) {
                AnsiConsole.MarkupLine($"[red]{ex.Message}[/]\n");
            }
        } else {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]\n");
        }
    }
}

