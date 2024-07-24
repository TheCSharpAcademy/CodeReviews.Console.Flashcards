using Flashcards.Repositories;
using Spectre.Console;

namespace Flashcards.Menu;
public static class StackManager {
    private static readonly StackRepository _stackRepository;

    static StackManager() {
        _stackRepository = new StackRepository(new AppDbContext());
    }

    public static bool Run() {
        while (true) {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an option:")
            .AddChoices(new[] {
                    "View stacks", "Go back to main menu"
            }));

            AnsiConsole.Clear();
            MainMenu.DisplayName();

            switch (choice) {
                case "View stacks":
                    var list = _stackRepository.GetStackNamesAsync();
                    foreach (var item in list) {
                        AnsiConsole.WriteLine(item);
                    }
                    break;
                case "Go back to main menu":
                    return false;
            }
        }
    }
}
