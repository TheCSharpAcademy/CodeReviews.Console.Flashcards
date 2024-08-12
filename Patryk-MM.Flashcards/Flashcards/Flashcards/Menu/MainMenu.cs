using Spectre.Console;

namespace Flashcards.Menu;
/// <summary>
/// Represents the main menu of the Flashcards application.
/// </summary>
public class MainMenu {
    private readonly StackManager _stackManager;
    private readonly StudySessionManager _studySessionManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainMenu"/> class.
    /// </summary>
    /// <param name="stackManager">The manager responsible for stack operations.</param>
    /// <param name="studySessionManager">The manager responsible for study sessions.</param>
    public MainMenu(StackManager stackManager, StudySessionManager studySessionManager) {
        _stackManager = stackManager;
        _studySessionManager = studySessionManager;
    }

    /// <summary>
    /// Displays the application name in a styled format.
    /// </summary>
    public static void DisplayName() {
        AnsiConsole.Write(
            new FigletText("Flashcards")
            .Centered()
            .Color(Color.Aqua));
    }

    /// <summary>
    /// Runs the main menu loop, allowing the user to select options to either study, manage stacks, or exit the application.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. Returns <c>true</c> if the application should continue running, otherwise <c>false</c>.</returns>
    public async Task RunAsync() {
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

                case "Manage stacks":
                    await _stackManager.RunAsync();
                    break;

                case "[red]Exit the app[/]":
                    return;
            }
        }
    }
}

