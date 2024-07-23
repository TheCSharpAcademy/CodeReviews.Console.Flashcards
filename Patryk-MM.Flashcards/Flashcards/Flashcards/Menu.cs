using Flashcards.Models;
using Flashcards.Repositories;
using Spectre.Console;

namespace Flashcards {
    public class Menu {
        private readonly AppDbContext _appDbContext;
        private readonly FlashcardRepository _flashcardRepository;

        public Menu() {
            _appDbContext = new AppDbContext();
            _flashcardRepository = new FlashcardRepository(_appDbContext);
        }
        public void DisplayName() {
            AnsiConsole.Write(
                new FigletText("Flashcards")
                .Centered()
                .Color(Color.Aqua));
        }

        public async Task<bool> RunAsync() {
            while (true) {
                var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Choose an option:")
                .AddChoices(new[] {
                    "Test", "Exit"
                }));

                AnsiConsole.Clear();
                DisplayName();

                switch (choice) {
                    case "Test":
                        try {
                            //var flashcard = await _flashcardRepository.GetByIdAsync(1);
                            //if (flashcard != null) {
                            //    AnsiConsole.WriteLine(flashcard.ToString());
                            //} else {
                            //    AnsiConsole.WriteLine("Flashcard not found.");
                            //}
                            var flashcards = await _flashcardRepository.GetAsync(f => f.Stack.Name == "Questions");
                            foreach (var flashcard in flashcards) {
                                AnsiConsole.WriteLine(flashcard.ToString());
                            }
                        } catch (Exception ex) {
                            AnsiConsole.WriteLine($"Failed to run {ex.Message}");
                        }
                        break;
                    case "Exit":
                        return false;
                }
                AnsiConsole.WriteLine("\n");
                AnsiConsole.Write(new Rule().RuleStyle("aqua"));
            }
        }
    }
}
