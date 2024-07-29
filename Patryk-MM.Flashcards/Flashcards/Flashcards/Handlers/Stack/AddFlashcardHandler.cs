
using Flashcards.Models;
using Flashcards.Repositories;
using Spectre.Console;

namespace Flashcards.Handlers.Stack;
public class AddFlashcardHandler : IStackActionHandler {
    private readonly IStackRepository _stackRepository;

    public AddFlashcardHandler(IStackRepository stackRepository) {
        _stackRepository = stackRepository;
    }

    public async Task<bool> HandleAsync(Models.Stack stack) {
        AnsiConsole.MarkupLine($"Adding a flashcard to stack: [aqua]{stack.Name}[/]");

        var flashcard = new Flashcard();

        flashcard.Question = AnsiConsole.Ask<string>("What's the question?");
        flashcard.Answer = AnsiConsole.Ask<string>("What's the answer?");

        if (AnsiConsole.Confirm("Are you sure to add the flashcard?")) {
            try {
                stack.Flashcards.Add(flashcard);
                await _stackRepository.EditAsync(stack);

                AnsiConsole.MarkupLine("[green]Flashcard successfully added![/]\n");
            }
            catch (Exception ex) {
                AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            }
        } else {
            AnsiConsole.MarkupLine("[yellow]Operation cancelled.[/]\n");
        }
        return true;
    }
}
