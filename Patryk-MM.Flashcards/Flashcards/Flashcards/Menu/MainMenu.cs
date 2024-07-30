using Spectre.Console;

namespace Flashcards.Menu;
public class MainMenu {
    private readonly FlashcardManager _flashcardManager;
    private readonly StackManager _stackManager;
    private readonly StudySessionManager _studySessionManager;

    public MainMenu(FlashcardManager flashcardManager, StackManager stackManager, StudySessionManager studySessionManager) {
        _flashcardManager = flashcardManager;
        _stackManager = stackManager;
        _studySessionManager = studySessionManager;
    }

    public static void DisplayName() {
        AnsiConsole.Write(
            new FigletText("Flashcards")
            .Centered()
            .Color(Color.Aqua));
    }

    public async Task<bool> Run() {
        DisplayName();
        while (true) {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[bold]Choose an option:[/]")
            .AddChoices(new[] {
                    "Study", "Manage stacks", "[red]Exit the app[/]"
            }));

            AnsiConsole.Clear();
            DisplayName();

            switch (choice) {
                case "Study":
                    await _studySessionManager.RunAsync();
                    break;
                //case "Manage flashcards":
                //    _flashcardManager.Run();
                //    break;
                case "Manage stacks":
                    await _stackManager.RunAsync();
                    break;
                case "[red]Exit the app[/]":
                    return false;
            }
            //AnsiConsole.WriteLine("\n");
            //AnsiConsole.Write(new Rule().RuleStyle("aqua")); //Aquamarine bar across the console
        }
    }
}

