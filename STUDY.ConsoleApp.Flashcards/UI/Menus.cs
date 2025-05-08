using Spectre.Console;
using STUDY.ConsoleApp.Flashcards.Enums;

namespace STUDY.ConsoleApp.Flashcards.UI;

public static class Menus
{
    public static void MainMenu()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var menuOptions = AnsiConsole.Prompt(new SelectionPrompt<MainMenuOptions>().Title("Main Menu")
                .AddChoices(Enum.GetValues<MainMenuOptions>()));

            switch (menuOptions)
            {
                case MainMenuOptions.ManageStacks:
                    ManageStack.Menu();
                    break;
                case MainMenuOptions.ManageFlashcards:
                    ManageFlashcards.Menu();
                    break;
                case MainMenuOptions.Study:
                    ManageStudySession.Menu();
                    break;
                case MainMenuOptions.ViewStudyHistory:
                    ViewStudySessionsHistory.Menu();
                    break;
                case MainMenuOptions.Exit:
                    AnsiConsole.MarkupLine("Exiting application...");
                    Environment.Exit(0);
                    break;
            }
        }
    }
}