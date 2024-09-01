using Spectre.Console;

namespace Flashcards;

internal class SelectionMenu
{
    internal void Menu()
    {
        bool closeApp = false;
        while (!closeApp)
        {
            Console.Clear();

            var selection = new Markup("Type 1 to Manage Stacks\n" +
                       "Type 2 to Manage Flashcards\n" +
                       "Type 3 to Start a Study Session\n" +
                       "Type 4 to See Previous Study Sessions\n" +
                       "Type 5 to Close the application");

            var panel = new Panel(selection)
            .Header("[Bold underline]Flashcards Menu[/]")
            .Border(BoxBorder.Ascii);

            AnsiConsole.Render(panel);

            Console.WriteLine("Select:\n");
            var command = Console.ReadLine();

            switch (command)
            {
                case "1":
                    StacksUI.ManageStacks();
                    break;
                case "2":
                    FlashcardUI.ManageFlashcards();
                    break;
                case "3":
                    StudySession.StartStudySession();
                    break;
                case "4":
                    StudySession.DisplayStudySessions();
                    break;
                case "5":
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                default:
                    AnsiConsole.MarkupLine("Invalid selection. Please press enter to try again.");
                    Console.ReadLine();
                    break;
            }
        }
    }
}
