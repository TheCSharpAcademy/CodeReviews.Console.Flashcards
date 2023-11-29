using DataAccess;
using DataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.SamGannon.UI
{
    internal class StackMenu
    {
        public static void ShowStackMenu(IDataAccess _dataAccess)
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
                    // ManageStacks();
                    break;
                case "3":
                    // MainMenu.ShowMenu();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                    // ShowStackMenu();
                    break;
            }

            void CreateStack()
            {
                List<DtoStack> listOfStackNames = new();
                Console.Clear();
                Console.WriteLine("Enter the name for your new stack:");
                string stackName = Console.ReadLine();
                string compareToName = stackName.Trim().ToUpper();
                int iAttempts = 0;

                while (string.IsNullOrEmpty(stackName)) 
                {
                    if(iAttempts < 3)
                    {
                        Console.WriteLine("Please enter a name for your new flashcard stack");
                        stackName = Console.ReadLine();
                        compareToName = stackName.Trim().ToUpper();
                        iAttempts++;
                    }
                    else
                    {
                        Console.WriteLine("You have exceeded the amount of attempts.");
                        Environment.Exit(1);
                    }
                }

                iAttempts = 0;
                listOfStackNames = _dataAccess.GetListOfStackNames();
                bool stackExists = CheckIfStackNameExist(listOfStackNames, compareToName);

                while (stackExists)
                {
                    if (iAttempts < 3)
                    {
                        Console.WriteLine($"A stack with the name '{stackName}' already exists. Please enter a different name");
                        stackName = Console.ReadLine();
                        compareToName = stackName.Trim().ToUpper();
                        stackExists = CheckIfStackNameExist(listOfStackNames, compareToName);
                        iAttempts++;
                    }
                    else
                    {
                        Console.WriteLine("You have exceeded the amount of attempts.");
                        Environment.Exit(1);
                    }
                    
                }

                iAttempts = 0;
                _dataAccess.AddStack(stackName);

                Console.WriteLine($"Stack '{stackName}' created successfully! Press a key to go back to the Main Menu.");
                Console.Read();
                MainMenu.ShowMenu(_dataAccess);
            }

            void ManageStacks()
            {
                Console.Clear();

                // TableVisualization.ShowTable();
                // List existing stacks, allow user to edit or delete stacks, etc.

                // After managing stacks, return to the stack menu
                // ShowStackMenu();
            }

        }

        private static bool CheckIfStackNameExist(List<DtoStack> stackNames, string compareToName)
        {
            foreach (DtoStack stack in stackNames)
            {
                stack.StackName = stack.StackName.Trim().ToUpper();
                if (stack.StackName == compareToName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
