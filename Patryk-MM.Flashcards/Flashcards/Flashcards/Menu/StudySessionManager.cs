using Flashcards.Repositories;
using Flashcards.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Menu;
public class StudySessionManager {
    private readonly StudySessionService _service;

    public StudySessionManager(StudySessionService service) {
        _service = service;
    }

    public async Task<bool> RunAsync() {
        while (true) {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an option:")
            .AddChoices(new[] {
                    "Start a study session", "View previous sessions", "Generate reports", "Go back to main menu"
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
                case "Go back to main menu":
                    return false;
            }
        }
    }
}

