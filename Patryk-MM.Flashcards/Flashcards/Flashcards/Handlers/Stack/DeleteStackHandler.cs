using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flashcards.Repositories;
using Spectre.Console;

namespace Flashcards.Handlers.Stack;
public class DeleteStackHandler : IStackActionHandler {
    private readonly IStackRepository _repository;
    public DeleteStackHandler(IStackRepository repository) {
        _repository = repository;
    }
    public async Task<bool> HandleAsync(Models.Stack stack) {
        var confirm = AnsiConsole.Confirm($"Are you sure you want to delete the stack '{stack.Name}'?");
        if (confirm) {
            await _repository.DeleteAsync(stack);
            AnsiConsole.MarkupLine($"Stack '[aqua]{stack.Name}[/]' has been deleted.");
            return false;
        }

        AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
        return true;

    }
}
