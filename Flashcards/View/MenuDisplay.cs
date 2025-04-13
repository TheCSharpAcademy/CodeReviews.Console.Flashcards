using Spectre.Console;
using System;
namespace Flashcards.View
{
    public static class MenuDisplay
    {
        public static string ViewMainMenu()
        {
            return DisplayMenu("MAIN MENU", new[]
            {
                "Start Study Session", "Manage Flashcard Stacks", "Manage Flashcards", "View Past Study Sessions", "Close Application"
            });
        }

        public static string ViewStackMenu()
        {
            return DisplayMenu("MANAGE FLASHCARD STACKS", new[]
            {
                "Create New Flashcard Stack", "Edit Existing Flashcard Stack", "View Flashcard Stacks", "Delete Flashcard Stack", "Return to Main Menu", "Close Application"
            });
        }

        public static string ViewFlashcardMenu()
        {
            return DisplayMenu("MANAGE FLASHCARDS", new[]
            {
                "Create New Flashcard", "Edit Existing Flashcard", "View Flashcards", "Delete Flashcard", "Return to Main Menu", "Close Application"
            });
        }

        private static string DisplayMenu(string title, string[] choices)
        {
            Console.Clear();
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title(title)
                .PageSize(10)
                .AddChoices(choices));
        }
    }
}
