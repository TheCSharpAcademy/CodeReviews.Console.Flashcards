using Flashcards.Tables;
using Spectre.Console;

namespace Flashcards;

internal class StacksUI
{
    public static void DisplayStacks()
    {
        Console.Clear();

        var stacks = Stacks.GetAllStacks();

        if (stacks.Count == 0)
        {
            Console.WriteLine("No available stacks. Please create a stack first. Press Enter to return the main menu.\n");
            Console.ReadLine();
            Utility.ReturnToMenu();
        }

        AnsiConsole.Write(new Markup("[bold underline]Available Stacks[/]\n"));

        var table = new Table();

        table.AddColumn("Stack");

        table.Border = TableBorder.Ascii;
        table.ShowHeaders = true;
        table.BorderColor(Color.Grey);
        table.ShowFooters = false;
        table.ShowRowSeparators();

        foreach (var stack in stacks)
        { 
            table.AddRow(stack.Name);
    
        }

        AnsiConsole.Write(table);
    }

    public static void IndividualStacks()
    {
        Console.Clear();
        
        DisplayStacks();

        Console.WriteLine("\nPlease type in the name of the stack that you would like to view, or enter 'M' to return to the main menu.\n");

        var stackName = Console.ReadLine().Trim();

        stackName = Utility.ValidString(stackName);

        if (stackName == "M" || stackName == "m")
        {
            Utility.ReturnToMenu();
        }

        var stackId = Stacks.ReturnStackID(stackName);

        if (stackId == 0)
        {
            Console.WriteLine("\nStack not found. Press any key to try again.\n");
            Console.ReadLine();
            IndividualStacks();
        }

        FlashcardUI.DisplayFlashcards(stackId);

        while (true)
        { 
            var selection = new Markup("Type 1 to Add flashcard to stack\n" +
                "Type 2 to Update flashcard in stack.\n" +
                "Type 3 to Delete flashcard instack.\n" +
                "Type 4 to Change to another stack.\n" +
                "Type 5 to Return to Main Menu.");

            Console.WriteLine("\nWould you like to add to or modify the cards of this stack? Choose a number below:\n");

            var panel = new Panel(selection)
                .Header("[Bold underline]Management of cards[/]")
                .Border(BoxBorder.Ascii);

            AnsiConsole.Render(panel);

            string input = Console.ReadLine().Trim();

           if (string.IsNullOrEmpty(input))
           {
                Console.WriteLine("Invalid input. Please enter a valid option.\n");
                continue;
           }
         
            if (int.TryParse(input, out int choice))
            {
                switch (choice)
                {
                    case 1:
                        FlashcardInput.CreateFlashcard(stackId);
                        break;
                    case 2:
                        FlashcardsTable.UpdateFlashcard(stackId);
                        break;
                    case 3:
                        FlashcardsTable.DeleteFlashcard(stackId);
                        break;
                    case 4:
                        IndividualStacks();
                        return;
                    case 5:
                        Utility.ReturnToMenu();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.\n");
                        continue;
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Please enter a valid number.\n");
            }
       }
    }

    public static void ManageStacks()
    {
        Console.Clear();
        var menu = new SelectionMenu();

        var selection = new Markup("Type 1 to Create a Stack\n" +
            "Type 2 to Delete a Stack\n" +
            "Type 3 to View Individual Stacks\n" +
            "Type 4 to Return to the Main Menu");

        var panel = new Panel(selection)
            .Header("[Bold underline]Stack Management[/]")
            .Border(BoxBorder.Ascii);

        AnsiConsole.Render(panel);

        Console.WriteLine("\nSelect:\n");

        string input = Console.ReadLine();

        if (int.TryParse(input, out int choice))
        {
            switch (choice)
            {
                case 1:
                    StacksInput.CreateStack();
                    break;
                case 2:
                    Stacks.DeleteStack();
                    break;
                case 3:
                    IndividualStacks();
                    break;
                case 4:
                    menu.Menu();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.\n");
                    ManageStacks();
                    break;
            }
        }
    }
}
