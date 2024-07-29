using Flashcards.Models;
using Flashcards.Repositories;
using Spectre.Console;

namespace Flashcards.Handlers.Stack;
public class DeleteFlashcardHandler : IStackActionHandler {

    private readonly IStackRepository _stackRepository;

    public DeleteFlashcardHandler(IStackRepository stackRepository) {
        _stackRepository = stackRepository;
    }

    public async Task<bool> HandleAsync(Models.Stack stack) {
        AnsiConsole.MarkupLine($"Deleting flashcard from stack: [aqua]{stack.Name}[/]");

        var flashcard = AnsiConsole.Prompt(
            new SelectionPrompt<Flashcard>()
            .Title("Choose a flashcard to delete (to cancel choose any flashcard and then choose \"no\"): ")
            .AddChoices(stack.Flashcards));

        if (AnsiConsole.Confirm("Are you sure to delete the flashcard?")) {
            try {
                stack.Flashcards.Remove(flashcard);
                await _stackRepository.EditAsync(stack);

                AnsiConsole.MarkupLine("[green]Flashcard deleted successfully![/]");
            }
            catch (Exception ex) {
                AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            }
        } else {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]");
        }
        return true;

    }
}
