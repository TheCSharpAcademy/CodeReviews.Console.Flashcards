using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.SamGannon.UI
{
    public class MainMenu
    {
        public static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Custom Flashcard Console App ===");
            Console.WriteLine("1. Flashcard Menu");
            Console.WriteLine("2. Stack Menu");
            Console.WriteLine("3. Start Study Session");
            Console.WriteLine("4. Exit");
            Console.WriteLine("Enter your choice (1, 2, 3, or 4): ");

            string choice = Console.ReadLine();

            while (choice != "1" && choice != "2" && choice != "3" && choice != "4")
            {
                Console.WriteLine("Invalid choice. Please enter 1, 2, 3, or 4.");
                choice = Console.ReadLine();
            }

            switch (choice)
            {
                case "1":
                    FlashcardMenu.ShowFlashcardMenu();
                    break;
                case "2":
                    StackMenu.ShowStackMenu();
                    break;
                case "3":
                    StudySession.StartStudySession();
                    break;
                case "4":
                    Console.WriteLine("Exiting the Custom Flashcard Console App. Goodbye!");
                    break;
                default:
                    break;
            }

        }
    }
}
 

