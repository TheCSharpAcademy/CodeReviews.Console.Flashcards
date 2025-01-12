using FlashCards.Control;
using FlashCards.Database;
using FlashCards.Models;

namespace FlashCards.View
{
    internal static class StackMenu
    {
        internal static void DisplayStackMenu()
        {
            while (true)
            {
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("STACK MENU");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("1. View all stacks");
                Console.WriteLine("2. Create a new stack");
                Console.WriteLine("3. Delete a stack");
                Console.WriteLine("4. Change stack name");
                Console.WriteLine("5. Return to main menu\n");
                Console.WriteLine("Choose an option from the menu.");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        List<Stack> stacks = StackDBOperations.GetStacks();
                        List<StackWithCleanId> cleanStacks = StackControl.LaunderStackId(stacks);
                        StackControl.ViewStacks(cleanStacks);
                        break;
                    case "2":
                        Console.Clear();
                        StackControl.CreateStack();
                        break;
                    case "3":
                        Console.Clear();
                        StackControl.DeleteStack();
                        break;
                    case "4":
                        Console.Clear();
                        StackControl.RenameStack();
                        break;
                    case "5":
                        Console.Clear();
                        return;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.\n");
                        break;
                }
            }
        }
    }
}
