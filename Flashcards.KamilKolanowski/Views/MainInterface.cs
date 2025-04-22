using Flashcards.KamilKolanowski.Controllers;
using Flashcards.KamilKolanowski.Enums;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Views;

internal class MainInterface
{
    internal void Start()
    {
        SessionController sessionController = new();
        while (true)
        {
            Console.Clear();

            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>().AddChoices(Options.MenuOptionsDisplay.Values)
            );

            var selectedOption = Options
                .MenuOptionsDisplay.FirstOrDefault(x => x.Value == menuChoice)
                .Key;

            switch (selectedOption)
            {
                case Options.MenuOptions.MFlashcards:
                    sessionController.ManageFlashcards();
                    break;
                case Options.MenuOptions.MStacks:
                    sessionController.ManageStacks();
                    break;
                case Options.MenuOptions.StudySession:
                    sessionController.ManageStudySession("study");
                    break;
                case Options.MenuOptions.ViewStudySessions:
                    sessionController.ManageStudySession("view");
                    break;
                case Options.MenuOptions.Exit:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
