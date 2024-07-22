using Spectre.Console;

namespace Flashcards.Arashi256.Classes
{
    internal class CommonUI
    {
        public static void Pause(string colour)
        {
            AnsiConsole.Markup($"[{colour}]Press any key to continue...[/]");
            Console.ReadKey(true);
        }
        public static int MenuOption(string question, int min, int max)
        {
            int selectedValue = 0;
            var userInput = AnsiConsole.Ask<int>(question);
            selectedValue = userInput;
            if (selectedValue < min || selectedValue > max)
            {
                AnsiConsole.MarkupLine("[red]Invalid input. Please enter a value within the specified range.[/]");
            }
            return selectedValue;
        }

        public static Utility.SortOrder GetSortOrderDialog()
        {
            AnsiConsole.MarkupLine("[white]Ascending (ASC): Orders data from earliest to latest.[/]");
            AnsiConsole.MarkupLine("[white]Descending (DESC): Orders data from latest to earliest.[/]");
            var choice = AnsiConsole.Prompt(
                            new SelectionPrompt<Utility.SortOrder>()
                                .Title("Please choose the sorting order:")
                                .AddChoices(Utility.SortOrder.ASC, Utility.SortOrder.DESC));
            AnsiConsole.MarkupLine($"You selected: [lime]{choice}[/]");
            return choice;
        }

        public static int GetNumberInput(string message, int min, int max)
        {
            AnsiConsole.MarkupLine("[white]Enter '0' to cancel[/]");
            int selection = AnsiConsole.Ask<int>(message);
            if (selection == 0) return -1;
            while (selection < min || selection > max)
            {
                AnsiConsole.MarkupLine("\n\n[red]Invalid number. Try again.[/]\n\n");
                selection = AnsiConsole.Ask<int>(message);
            }
            return selection;
        }
    }
}
