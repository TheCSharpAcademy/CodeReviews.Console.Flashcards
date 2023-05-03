using LucianoNicolasArrieta.Flashcards.Model;
using LucianoNicolasArrieta.Flashcards.Persistence;
using System.ComponentModel.DataAnnotations;

namespace LucianoNicolasArrieta.Flashcards
{
    public class Menu
    {
        private void PrintMainMenu()
        {
            Console.WriteLine("\n--------------- Main Menu ---------------");
            Console.WriteLine(@"Choose an option:
1. Manage Stacks
2. Study
3. View previous Study Sessions
0. Exit
----------------------------------");
        }

        private void PrintStacksManagementMenu()
        {
            Console.WriteLine("\n--------------- Stacks Menu ---------------");
            Console.WriteLine(@"Choose an option:
1. Select a Stack
2. Create a Stack
3. Delete a Stack
0. Return back to main menu
----------------------------------");
        }

        private void PrintStackMenu(string subject)
        {
            Console.WriteLine($"\n--------------- {subject} ---------------");
            Console.WriteLine(@"Choose an option:
1. Change the current Stack
2. View all Flashcards in the Stack
3. View X amount of Flashcards in the Stack
4. Create Flashcard in the current Stack
5. Edit a Flashcard
6. Delete a Flashcard
0. Return back to main menu
----------------------------------");
        }

        public void RunMainMenu()
        {
            bool closeApp = false;
            string user_opt;

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
                        // Study
                        StackRepository stackRepo = new StackRepository();
                        InputValidator validator = new InputValidator();
                        stackRepo.PrintAll();
                        Console.WriteLine("Enter the id of the stack you want to study or 0 to cancel the operation:");
                        int selected_id = validator.IdInput();
                        Stack selected = stackRepo.GetUIStack(selected_id);
                        Console.Clear();
                        StudySession session = new StudySession();
                        session.RunSession(selected);
                        break;
                    case "3":
                        // View study sessions data
                        StudySessionRepository studySessionRepo = new StudySessionRepository();
                        studySessionRepo.PrintAll();
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Error: Invalid option. Try again");
                        break;
                }
            }
        }

        public void ManageStacks()
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
                        stackRepo.PrintAll();
                        Console.WriteLine("Enter the id of the stack you want to select or 0 to cancel the operation:");
                        int selected_id = validator.IdInput();
                        Stack selected = stackRepo.GetUIStack(selected_id);
                        Console.Clear();
                        RunStackMenu(selected);
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

                        Console.WriteLine("Enter the id of the Stack you want to delete or 0 to cancel the operation.");
                        Console.WriteLine("Remember: All the flashcards inside that stack will be deleted too.");
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

        public void RunStackMenu(Stack stack)
        {
            bool returnBack = false;
            string user_opt;
            StackRepository stackRepo = new StackRepository();
            FlashcardRepository flashcardRepo = new FlashcardRepository();
            InputValidator validator = new InputValidator();

            while (!returnBack)
            {
                PrintStackMenu(stack.Subject);
                user_opt = Console.ReadLine().Trim();

                switch (user_opt)
                {
                    case "0":
                        returnBack = true;
                        Console.Clear();
                        RunMainMenu();
                        break;
                    case "1":
                        // Change the current Stack
                        stackRepo.PrintAll();
                        Console.WriteLine("Enter the id of the stack you want to select or 0 to cancel the operation:");
                        int selected_id = validator.IdInput();
                        Stack selected = stackRepo.GetUIStack(selected_id);
                        Console.Clear();
                        RunStackMenu(selected);
                        break;
                    case "2":
                        // View all flashcards
                        flashcardRepo.PrintAllFromStack(stack.Id);
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "3":
                        // View X amount flashcards
                        Console.WriteLine("Enter the amount of Flashcards you want to see:");
                        int amount = validator.IdInput();
                        flashcardRepo.PrintXAmount(amount, stack.Id);
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "4":
                        // Create a flashcard
                        Console.WriteLine("Enter the Question of the card or 0 to cancel the operation:");
                        string question = validator.StringInput();
                        Console.WriteLine("Enter the Answer of the card or 0 to cancel the operation:");
                        string answer = validator.StringInput();
                        Flashcard newFlashcard = new Flashcard(question, answer);
                        flashcardRepo.Insert(newFlashcard, stack.Id);
                        break;
                    case "5":
                        // Edit a flashcard
                        flashcardRepo.PrintAllFromStack(stack.Id);
                        Console.WriteLine("Enter the id of the Flashcard you want to update or 0 to cancel the operation:");
                        int id_to_update = validator.IdInput();
                        Console.WriteLine("Enter the new Question of the card or 0 to cancel the operation:");
                        string new_question = validator.StringInput();
                        Console.WriteLine("Enter the new Answer of the card or 0 to cancel the operation:");
                        string new_answer = validator.StringInput();
                        flashcardRepo.Update(id_to_update, stack.Id, new_question, new_answer);
                        break;
                    case "6":
                        // Delete a flashcard
                        flashcardRepo.PrintAllFromStack(stack.Id);
                        Console.WriteLine("Enter the id of the Flashcard you want to delete or 0 to cancel the operation:");
                        int id_to_delete = validator.IdInput();
                        flashcardRepo.Delete(id_to_delete, stack.Id);
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
