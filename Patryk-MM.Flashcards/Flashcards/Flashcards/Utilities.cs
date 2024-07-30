using Flashcards.Menu;
using Spectre.Console;

namespace Flashcards;
public static class Utilities {
    public static void ClearConsole() {
        AnsiConsole.Clear();
        MainMenu.DisplayName();
    }
}
