using Flashcards.Repositories;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.Menu;
public class StackManager {
    private readonly StackService _service;

    public StackManager(StackService service) {
        _service = service;
    }

    public async Task<bool> RunAsync() {
        while (true) {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an option:")
            .AddChoices(new[] { "Test",
                    "View stacks", "Go back to main menu"
            }));

            AnsiConsole.Clear();
            MainMenu.DisplayName();

            switch (choice) {
                case "View stacks":
                    await _service.ViewStacks();
                    break;
                case "Go back to main menu":
                    return false;
            }
        }
    }
}
