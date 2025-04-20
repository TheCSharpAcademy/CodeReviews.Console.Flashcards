using Flashcards.KamilKolanowski.Enums;
using Flashcards.KamilKolanowski.Controllers;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Views;

internal class MainInterface
{
    internal static void Start()
    {
        while (true)
        {
            Console.Clear();

            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(Options.MenuOptionsDisplay.Values));

            var selectedOption = Options.MenuOptionsDisplay
                .FirstOrDefault(x => x.Value == menuChoice).Key;

            switch (selectedOption)
            {
                case Options.MenuOptions.MFlashcards:
                    SessionController.ManageFlashcards();
                    break;
                case Options.MenuOptions.MStacks:
                    SessionController.ManageStacks();
                    break;
                case Options.MenuOptions.StudySession:
                    SessionController.ManageStudySession("study");
                    break;
                case Options.MenuOptions.ViewStudySessions:
                    SessionController.ManageStudySession("view");
                    break;
                case Options.MenuOptions.Exit:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}