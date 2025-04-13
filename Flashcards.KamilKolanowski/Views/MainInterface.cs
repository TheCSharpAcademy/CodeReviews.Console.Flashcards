using Flashcards.KamilKolanowski.Data;
using Flashcards.KamilKolanowski.Enums;
using Flashcards.KamilKolanowski.Services;
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
                    Console.WriteLine("Managing Stacks...");
                    Console.ReadKey();
                    break;
                case Options.MenuOptions.StudySession:
                    Console.WriteLine("Starting Study Session...");
                    Console.ReadKey();
                    break;
                case Options.MenuOptions.ViewStudySessions:
                    Console.WriteLine("Viewing Study Sessions...");
                    Console.ReadKey();
                    break;
                case Options.MenuOptions.Exit:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}