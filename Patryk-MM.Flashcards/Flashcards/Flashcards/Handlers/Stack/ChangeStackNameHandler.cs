using Flashcards.Models;
using Flashcards.Repositories;
using Spectre.Console;

namespace Flashcards.Handlers.Stack;
public class ChangeStackNameHandler : IStackActionHandler {
    private readonly IStackRepository _repository;

    public ChangeStackNameHandler(IStackRepository repository) {
        _repository = repository;
    }

    public async Task<bool> HandleAsync(Models.Stack stack) {
        var stackNames = await _repository.GetStackNamesAsync();
        var newName = AnsiConsole.Ask<string>("Enter the new name for the stack or input [red]cancel[/]:");

        if (newName == "cancel") {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
            return true;
        }

        if (stackNames.Contains(newName)) {
            AnsiConsole.WriteLine($"The name '{newName}' is already taken. [yellow]Operation cancelled.[/]");
        } else {
            stack.Name = newName;
            await _repository.EditAsync(stack);
            AnsiConsole.MarkupLine($"[green]Stack name changed to:[/] [aqua]{newName}[/]");
        }
        return true;
    }
}

