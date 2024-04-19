// Flashcards study console application
// Check ReadMe for more information

using DbHelpers.HopelessCoding;
using FlashCards.HopelessCoding.Flashcards;
using FlashCards.HopelessCoding.Stacks;
using FlashCards.HopelessCoding.Study;
using HelperMethods.HopelessCoding;
using Spectre.Console;

class Program
{
    static void Main()
    {
        DatabaseHelpers.InitializeDatabase();

        while (true) {
        Console.Clear();
        AnsiConsole.Write(new Markup("[yellow1]MAIN MENU\n\n[/]"));
        var selection = Helpers.MenuSelector(["Manage Stacks", "Manage Flashcards", "Study", "Exit"]);

            switch (selection)
            {
                case "Manage Stacks":
                    StacksMenu.StacksMainMenu();
                    break;
                case "Manage Flashcards":
                    FlashcardsMenu.SelectStackForFlashcardsMenu();
                    break;
                case "Study":
                    StudyMenu.StudyMainMenu();
                    break;
                case "Exit":
                    AnsiConsole.Write(new Markup($"[yellow1]Application is closing...\n[/]"));
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
