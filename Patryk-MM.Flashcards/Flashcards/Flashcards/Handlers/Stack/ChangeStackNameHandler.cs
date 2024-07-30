using Flashcards.Repositories;
using Spectre.Console;

namespace Flashcards.Handlers.Stack;
/// <summary>
/// Handles the renaming of a stack.
/// </summary>
public class ChangeStackNameHandler : IStackActionHandler {
    private readonly IStackRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeStackNameHandler"/> class.
    /// </summary>
    /// <param name="repository">The stack repository to interact with.</param>
    public ChangeStackNameHandler(IStackRepository repository) {
        _repository = repository;
    }

    /// <summary>
    /// Handles the asynchronous renaming of the specified stack.
    /// </summary>
    /// <param name="stack">The stack whose name will be changed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(Models.Stack stack) {
        var stackNames = await _repository.GetStackNamesAsync();
        var newName = AnsiConsole.Ask<string>("Enter the new name for the stack or input [red]cancel[/]:");

        if (newName == "cancel") {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
            return;
        }

        if (stackNames.Contains(newName)) {
            AnsiConsole.MarkupLine($"The name '{newName}' is already taken. [yellow]Operation cancelled.[/]");
        } else {
            stack.Name = newName;
            await _repository.EditAsync(stack);
            AnsiConsole.MarkupLine($"[green]Stack name changed to:[/] [aqua]{newName}[/]");
        }
    }
}
