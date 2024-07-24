using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Menu;
public static class StudySessionManager {
    public static bool Run() {
        while (true) {
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an option:")
            .AddChoices(new[] {
                    "Start a study session", "Go back to main menu"
            }));

            AnsiConsole.Clear();
            MainMenu.DisplayName();

            switch (choice) {
                case "Start a study session":
                    AnsiConsole.WriteLine("Study session.");
                    break;
                case "Go back to main menu":
                    return false;
            }
        }
    }
}

