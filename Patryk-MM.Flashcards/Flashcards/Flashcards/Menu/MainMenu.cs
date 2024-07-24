using Flashcards.Models;
using Flashcards.Repositories;
using Spectre.Console;

namespace Flashcards.Menu;
public static class MainMenu {
    private static readonly AppDbContext _appDbContext;
    private static readonly FlashcardRepository _flashcardRepository;

    static MainMenu() {
        _appDbContext = new AppDbContext();
        _flashcardRepository = new FlashcardRepository(_appDbContext);
    }
    public static void DisplayName() {
        AnsiConsole.Write(
            new FigletText("Flashcards")
            .Centered()
            .Color(Color.Aqua));
    }

    public static async Task<bool> RunAsync() {
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
                case "Test":
                    try {
                        var flashcards = await _flashcardRepository.GetAsync(f => f.Stack.Name == "Questions");
                        foreach (var flashcard in flashcards) {
                            AnsiConsole.WriteLine(flashcard.ToString());
                        }
                    }
                    catch (Exception ex) {
                        AnsiConsole.WriteLine($"Failed to run {ex.Message}");
                    }
                    break;
                case "Study":
                    StudySessionManager.Run();
                    break;
                case "Manage flashcards":
                    FlashcardManager.Run();
                    break;
                case "Manage stacks":
                    StackManager.Run();
                    break;
                case "Exit":
                    return false;
            }
            AnsiConsole.WriteLine("\n");
            AnsiConsole.Write(new Rule().RuleStyle("aqua")); //Aquamarine bar across the console
        }
    }
}

