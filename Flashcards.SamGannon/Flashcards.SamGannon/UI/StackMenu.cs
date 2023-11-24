using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.SamGannon.UI
{
    internal class StackMenu
    {
        public static void ShowStackMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Stack Menu ===");
            Console.WriteLine("1. Create Stack");
            Console.WriteLine("2. Manage Stacks");
            Console.WriteLine("3. Back to Main Menu");
            Console.WriteLine("Enter your choice (1, 2, or 3): ");

            string choice = Console.ReadLine();

            while (choice != "1" && choice != "2" && choice != "3")
            {
                Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                choice = Console.ReadLine();
            };

            switch (choice)
            {
                case "1":
                    CreateStack();
                    break;
                case "2":
                    ManageStacks();
                    break;
                case "3":
                    MainMenu.ShowMenu();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                    ShowStackMenu();
                    break;
            }
        }

        private static void CreateStack()
        {
            Console.WriteLine("Enter the name for your new stack:");
            string stackName = Console.ReadLine();

            // Call DatabaseService method to add the new stack to the database
            // DatabaseService.AddStack(stackName);

            Console.WriteLine($"Stack '{stackName}' created successfully!");
            ShowStackMenu();
        }

        private static void ManageStacks()
        {
            Console.WriteLine("=== Manage Stacks ===");
            // List existing stacks, allow user to edit or delete stacks, etc.

            // After managing stacks, return to the stack menu
            ShowStackMenu();
        }
    }
}
