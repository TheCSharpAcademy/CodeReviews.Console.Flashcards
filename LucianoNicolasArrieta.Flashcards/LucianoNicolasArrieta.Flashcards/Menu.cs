using LucianoNicolasArrieta.Flashcards.Model;
using LucianoNicolasArrieta.Flashcards.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucianoNicolasArrieta.Flashcards
{
    public class Menu
    {

        private void PrintMainMenu()
        {
            Console.WriteLine("\n---------------Main Menu---------------");
            Console.WriteLine(@"Choose an option:
1. Manage Stacks
2. Manage Flashcards
3. Study
4. View previous Study Sessions
0. Exit
----------------------------------");
        }

        private void PrintStacksManagementMenu()
        {
            Console.WriteLine("\n---------------Stacks Menu---------------");
            Console.WriteLine(@"Choose an option:
1. Select a Stack
2. Create a Stack
3. Delete a Stack
0. Return back to main menu
----------------------------------");
        }

        private void PrintFlashcardsManagementMenu()
        {
            Console.WriteLine("\n---------------Flashcards Menu---------------");
            Console.WriteLine(@"Choose an option:
1. View Flashcards
2. Delete a Flashcard
0. Return back to main menu
----------------------------------");
        }

        public void RunMain()
        {
            bool closeApp = false;
            string user_opt;
            //UserInput userInput = new UserInput();
            while (!closeApp)
            {
                PrintMainMenu();
                user_opt = Console.ReadLine().Trim();

                switch (user_opt)
                {
                    case "0":
                        closeApp = true;
                        Console.WriteLine("See you!");
                        Environment.Exit(0);
                        break;
                    case "1":
                        //Manage Stacks
                        Console.Clear();
                        ManageStacks();
                        break;
                    case "2":
                        // Manage Flashcards
                        Console.Clear();
                        ManageFlashcards();
                        break;
                    case "3":
                        // Study
                        break;
                    case "4":
                        // View study sessions data
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Error: Invalid option. Try again");
                        break;
                }
            }
        }

        private void ManageStacks()
        {
            bool returnBack = false;
            string user_opt;
            StackRepository stackrepo = new StackRepository();
            while (!returnBack)
            {
                PrintStacksManagementMenu();
                user_opt = Console.ReadLine().Trim();

                switch (user_opt)
                {
                    case "0":
                        returnBack = true;
                        Console.Clear();
                        break;
                    case "1":
                        // Select a stack
                        // Print stacks, select one of them
                        break;
                    case "2":
                        // Create a stack
                        Console.WriteLine("Please enter the subject of the new Stack:");
                        string stackSubject = Console.ReadLine();
                        Stack newStack = new Stack(stackSubject);
                        stackrepo.Insert(newStack);
                        break;
                    case "3":
                        // Delete a stack
                        // Print stacks, select one of them
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Error: Invalid option. Try again");
                        break;
                }
            }
        }

        private void ManageFlashcards()
        {
            bool returnBack = false;
            string user_opt;

            while (!returnBack)
            {
                PrintFlashcardsManagementMenu();
                user_opt = Console.ReadLine().Trim();

                switch (user_opt)
                {
                    case "0":
                        returnBack = true;
                        Console.Clear();
                        break;
                    case "1":
                        // View flashcards
                        // Print flashcards
                        break;
                    case "2":
                        // Delete a flashcard
                        // Print flashcards, Select one of them
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Error: Invalid option. Try again");
                        break;
                }
            }
        }
    }
}
