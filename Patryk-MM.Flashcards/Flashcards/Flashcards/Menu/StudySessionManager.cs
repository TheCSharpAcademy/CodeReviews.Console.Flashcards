using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.Menu;

/// <summary>
/// Manages study session operations within the Flashcards application.
/// </summary>
public class StudySessionManager {
    private readonly StudySessionService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="StudySessionManager"/> class.
    /// </summary>
    /// <param name="service">The service responsible for handling study session operations.</param>
    public StudySessionManager(StudySessionService service) {
        _service = service;
    }

    /// <summary>
    /// Runs the study session management menu asynchronously, allowing the user to choose options for starting
    /// a study session, viewing previous sessions, generating reports, or returning to the main menu.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. Returns <c>true</c> if the user chooses to continue, otherwise <c>false</c> when opting to return to the main menu.</returns>
    public async Task RunAsync() {
        while (true) {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an option:")
            .AddChoices(new[] {
                    "Start a study session", "View previous sessions", "Generate reports", "[red]Go back to main menu[/]"
            }));

            AnsiConsole.Clear();
            MainMenu.DisplayName();

            switch (choice) {
                case "Start a study session":
                    await _service.StartSession();
                    break;
                case "View previous sessions":
                    await _service.ViewSessions();
                    break;
                case "Generate reports":
                    await _service.GenerateReports();
                    break;
                case "[red]Go back to main menu[/]":
                    return;
            }
        }
    }
}
