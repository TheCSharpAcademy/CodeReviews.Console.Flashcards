using FlashCards.Control;

namespace FlashCards.View
{
    internal static class FlashCardMenu
    {
        internal static void DisplayFlashCardMenu()
        {
            while (true)
            {
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("FLASHCARD MENU");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("1. View flashcards");
                Console.WriteLine("2. Add flashcards");
                Console.WriteLine("3. Delete flashcard");
                Console.WriteLine("4. Edit flashcard");
                Console.WriteLine("5. Return to main menu\n");
                Console.WriteLine("Choose an option from the menu.");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        FlashCardControl.ViewFlashCard();
                        break;
                    case "2":
                        Console.Clear();
                        FlashCardControl.AddFlashCard();
                        break;
                    case "3":
                        FlashCardControl.DeleteFlashCard();
                        break;
                    case "4":
                        FlashCardControl.EditFlashCard();
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
