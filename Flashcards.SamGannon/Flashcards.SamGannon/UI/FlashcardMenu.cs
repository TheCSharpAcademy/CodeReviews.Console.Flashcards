using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.SamGannon.UI
{
    internal class FlashcardMenu
    {
        public static void ShowFlashcardMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Flashcard Menu ===");
            Console.WriteLine("1. Create Flashcards");
            Console.WriteLine("2. Manage Flashcards");
            Console.WriteLine("3. Back to Main Menu");
            Console.WriteLine("Enter your choice (1, 2, or 3): ");

            string choice = Console.ReadLine();

            while (choice != "1" && choice != "2" && choice != "3")
            {
                Console.WriteLine("Invalid choice. Please enter 1, 2, 3, or 4.");
                choice = Console.ReadLine();
            }

            switch (choice)
            {
                case "1":
                    CreateFlashcardsMenu();
                    break;
                case "2":
                    ManageFlashcards();
                    break;
                case "3":
                    MainMenu.ShowMenu();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                    ShowFlashcardMenu();
                    break;
            }
        }

        private static void CreateFlashcardsMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Create Flashcards ===");
            Console.WriteLine("Enter the question for the new flashcard:");
            string question = Console.ReadLine();

            Console.WriteLine("Enter the answer for the new flashcard:");
            string answer = Console.ReadLine();

            // Call DatabaseService method to add the new flashcard to the database
            // DatabaseService.AddFlashcard(question, answer);

            Console.WriteLine("Flashcard created successfully!");
            ShowFlashcardMenu();
        }

        private static void ManageFlashcards()
        {
            Console.Clear();
            Console.WriteLine("=== Manage Flashcards ===");
            // List flashcards, allow user to edit or delete flashcards, etc.

            // After managing flashcards, return to the flashcard menu
            ShowFlashcardMenu();
        }
    }
}
