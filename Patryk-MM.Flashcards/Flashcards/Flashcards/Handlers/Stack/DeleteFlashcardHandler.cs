using Flashcards.Models;
using Flashcards.Repositories;
using Spectre.Console;

namespace Flashcards.Handlers.Stack;
/// <summary>
/// Handles the deletion of flashcards from a stack.
/// </summary>
public class DeleteFlashcardHandler : IStackActionHandler {

    private readonly IStackRepository _stackRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteFlashcardHandler"/> class.
    /// </summary>
    /// <param name="stackRepository">The repository to interact with stack data.</param>
    public DeleteFlashcardHandler(IStackRepository stackRepository) {
        _stackRepository = stackRepository;
    }

    /// <summary>
    /// Handles the asynchronous operation of deleting a flashcard from the specified stack.
    /// </summary>
    /// <param name="stack">The stack from which the flashcard will be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(Models.Stack stack) {
        AnsiConsole.MarkupLine($"Deleting flashcard from stack: [aqua]{stack.Name}[/]");

        if (stack.Flashcards.Count == 0) {
            AnsiConsole.MarkupLine($"[yellow]There are no flashcards in this stack.[/]\n");
            return;
        }

        var flashcard = AnsiConsole.Prompt(
            new SelectionPrompt<Flashcard>()
            .Title("Choose a flashcard to delete (to cancel choose any flashcard and then choose \"no\"): ")
            .AddChoices(stack.Flashcards));

        if (AnsiConsole.Confirm("Are you sure to delete the flashcard?")) {
            try {
                stack.Flashcards.Remove(flashcard);
                await _stackRepository.EditAsync(stack);

                AnsiConsole.MarkupLine("[green]Flashcard deleted successfully![/]\n");
            } catch (Exception ex) {
                AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            }
        } else {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]\n");
        }
    }
}

