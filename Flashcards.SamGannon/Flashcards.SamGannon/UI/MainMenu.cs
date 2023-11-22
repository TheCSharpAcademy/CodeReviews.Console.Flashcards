using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.SamGannon.UI
{
    internal class MainMenu
    {
        public static void ShowMenu()
        {
            Console.WriteLine("=== Custom Flashcard Console App ===");
            Console.WriteLine("1. Flashcard Menu");
            Console.WriteLine("2. Stack Menu");
            Console.WriteLine("3. Start Study Session");
            Console.WriteLine("4. Exit");

            Console.Write("Enter your choice (1, 2, 3, or 4): ");
            string choice = Console.ReadLine();

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
                    Console.WriteLine("Invalid choice. Please enter 1, 2, 3, or 4.");
                    ShowMenu();
                    break;
            }
        }
    }
}
    }
}
