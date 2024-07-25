using Spectre.Console;

namespace Flashcards.Menu;
public class MainMenu {
    private readonly FlashcardManager _flashcardManager;
    private readonly StackManager _stackManager;

    public MainMenu(FlashcardManager flashcardManager, StackManager stackManager) {
        _flashcardManager = flashcardManager;
        _stackManager = stackManager;
    }

    public static void DisplayName() {
        AnsiConsole.Write(
            new FigletText("Flashcards")
            .Centered()
            .Color(Color.Aqua));
    }

    public async Task<bool> Run() {
        while (true) {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an option:")
            .AddChoices(new[] {
                    "Test", "Study", "Manage flashcards", "Manage stacks", "Exit"
            }));

            AnsiConsole.Clear();
            DisplayName();

            switch (choice) {
                case "Study":
                    StudySessionManager.Run();
                    break;
                case "Manage flashcards":
                    _flashcardManager.Run();
                    break;
                case "Manage stacks":
                    await _stackManager.RunAsync();
                    break;
                case "Exit":
                    return false;
            }
            AnsiConsole.WriteLine("\n");
            AnsiConsole.Write(new Rule().RuleStyle("aqua")); //Aquamarine bar across the console
        }
    }
}

