using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.Menu;
public class FlashcardManager {
    private readonly FlashcardService _service;

    public FlashcardManager(FlashcardService service) {
        _service = service;
    }

    public bool Run() {
        while (true) {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an option:")
            .AddChoices(new[] {
                    "View flashcards", "Go back to main menu"
            }));

            AnsiConsole.Clear();
            MainMenu.DisplayName();

            switch (choice) {
                case "View flashcards":
                    AnsiConsole.WriteLine("Test completed.");
                    break;
                case "Go back to main menu":
                    return false;
            }
        }
    }
}
