using Flashcards.KamilKolanowski.Data;
using Flashcards.KamilKolanowski.Enums;
using Spectre.Console;

namespace Flashcards.KamilKolanowski.Views;

internal class MainInterface
{
    internal static void Start()
    {
        DatabaseManager _databaseManager = new();
        
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
                    foreach (var row in _databaseManager.ReadTable<Cards>("Cards"))
                    {
                        Console.WriteLine($"FlashcardId: {row.FlashcardId} | FlashcardTitle: {row.FlashcardTitle} | FlashcardContent: {row.FlashcardContent}");
                    }
                    Console.ReadKey();
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