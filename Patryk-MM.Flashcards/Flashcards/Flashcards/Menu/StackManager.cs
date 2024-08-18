using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.Menu;
/// <summary>
/// Manages stack-related operations in the Flashcards application.
/// </summary>
public class StackManager {
    private readonly StackService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="StackManager"/> class.
    /// </summary>
    /// <param name="service">The service responsible for stack operations.</param>
    public StackManager(StackService service) {
        _service = service;
    }

    /// <summary>
    /// Runs the stack management menu asynchronously, allowing the user to choose options for viewing,
    /// adding, or managing stacks, or returning to the main menu.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RunAsync() {
        while (true) {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose an option:")
                    .AddChoices(new[] {
                            "View stacks", "Add a stack", "Manage a stack", "[red]Go back to main menu[/]"
                    }));

            AnsiConsole.Clear();
            MainMenu.DisplayName();

            switch (choice) {
                case "View stacks":
                    await _service.ViewStacks();
                    break;
                case "Add a stack":
                    await _service.AddStack();
                    break;
                case "Manage a stack":
                    await _service.ManageStack();
                    break;
                case "[red]Go back to main menu[/]":
                    return;
            }
        }
    }
}
