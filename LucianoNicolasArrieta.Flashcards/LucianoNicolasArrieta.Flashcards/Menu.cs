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
            StackRepository stackRepo = new StackRepository();
            InputValidator validator = new InputValidator();
            while (!returnBack)
            {
                PrintStacksManagementMenu();
                user_opt = Console.ReadLine().Trim();

                switch (user_opt)
                {
                    case "0":
                        // Go back
                        returnBack = true;
                        Console.Clear();
                        break;
                    case "1":
                        // Select a stack
                        // Print stacks, select one of them
                        // TODO: Menu of the stack selected
                        stackRepo.PrintAll();
                        Console.WriteLine("Enter the id of the stack you want to select or 0 to cancel the operation:");
                        int selected_id = validator.IdInput();
                        Stack selected = stackRepo.SelectStack(selected_id);
                        Console.WriteLine($"You selected {selected.Subject}!");
                        break;
                    case "2":
                        // Create a stack
                        Console.WriteLine("Enter the subject of the new Stack or 0 to cancel the operation:");
                        string stackSubject = validator.StringInput();
                        Stack newStack = new Stack(stackSubject);
                        stackRepo.Insert(newStack);
                        break;
                    case "3":
                        // Delete a stack
                        stackRepo.PrintAll();
                        Console.WriteLine("Enter the id of the Stack you want to delete or 0 to cancel the operation:");
                        int id_to_delete = validator.IdInput();
                        stackRepo.Delete(id_to_delete);
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
