using Flashcards.Database;
using Flashcards.Validation;
using Flashcards.View;
namespace Flashcards.Model
{
    internal class UserInputManager
    {
        public static void Menu()
        {
            string? menuChoice;

            do
            {
                menuChoice = MenuDisplay.ViewMainMenu();
                switch (menuChoice)
                {                   
                    case "Start Study Session":
                        StudyManager.StartStudySession();
                        break;
                    case "Manage Flashcard Stacks":
                        StackMenu();
                        break;
                    case "Manage Flashcards":
                        Flashcards();
                        break;
                    case "View Past Study Sessions":
                        StudyManager.ViewStudySessions();
                        break;
                    case "Close Application":
                        Console.WriteLine("Exiting the application. Goodbye!");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                ReturnToMenu("Main");
            } while (menuChoice != "Close App");
        }

        public static void StackMenu()
        {
            string? menuChoice;

            do
            {
                menuChoice = MenuDisplay.ViewStackMenu();
                switch (menuChoice)
                {                  
                    case "Create New Flashcard Stack":
                        StackManager.CreateStack();
                        break;
                    case "Edit Existing Flashcard Stack":
                        StackManager.UpdateStack();
                        break;
                    case "View Flashcard Stacks":
                        StackManager.ViewFlashcardStacks();
                        break;
                    case "Delete Flashcard Stack":
                        StackManager.DeleteStack();
                        break;                    
                    case "Return to Main Menu":
                        Menu();
                        break;
                    case "Close Application":
                        Console.WriteLine("Exiting the application. Goodbye!");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                ReturnToMenu("Flashcard Stack");
            } while (menuChoice != "Close App");
        }

        public static void Flashcards()
        {
            if (!DatabaseManager.DoWeHaveFlashcardStacks())
            {
                Console.WriteLine("No flashcard stacks found. Please create a stack first.");
                return;
            }

            FlashcardMenu();
        }
        public static void FlashcardMenu()
        {         
            string? menuChoice;

            do
            {
                menuChoice = MenuDisplay.ViewFlashcardMenu();
                switch (menuChoice)
                {
                    case "Create New Flashcard":
                        FlashcardManager.CreateFlashcard();
                        break;
                    case "Edit Existing Flashcard":
                        FlashcardManager.UpdateFlashcard();
                        break;
                    case "View Flashcards":
                        FlashcardManager.ViewFlashcards();
                        break;
                    case "Delete Flashcard":
                        FlashcardManager.DeleteFlashcard();
                        break;
                    case "Return to Main Menu":
                        Menu();
                        break;
                    case "Close Application":
                        Console.WriteLine("Exiting the application. Goodbye!");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                ReturnToMenu("Flashcard");
            } while (menuChoice != "Close App");
        }

        internal static void ReturnToMenu(string menuName)
        {
            Console.Write($"\nPress any key to return to the {menuName} Menu...");
            Console.ReadKey();
        }
    }

}
