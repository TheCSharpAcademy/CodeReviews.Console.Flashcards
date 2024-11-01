using Flashcards.Repositories;
using Spectre.Console;

namespace Flashcards.Handlers.Stack;
/// <summary>
/// Handles the deletion of stacks.
/// </summary>
public class DeleteStackHandler : IStackActionHandler {
    private readonly IStackRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteStackHandler"/> class.
    /// </summary>
    /// <param name="repository">The repository to interact with stack data.</param>
    public DeleteStackHandler(IStackRepository repository) {
        _repository = repository;
    }

    /// <summary>
    /// Handles the asynchronous operation of deleting the specified stack.
    /// </summary>
    /// <param name="stack">The stack to be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(Models.Stack stack) {
        var confirm = AnsiConsole.Confirm($"Are you sure you want to delete the stack '{stack.Name}'?");
        if (confirm) {
            await _repository.DeleteAsync(stack);
            AnsiConsole.MarkupLine($"Stack '[aqua]{stack.Name}[/]' has been deleted.");
            return;
        }

        AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
    }
}
