namespace Flashcards.jkjones98;

internal class FlashcardsMngMenu
{
    internal void DisplayFlashcardMenu()
    {
        bool rtnMainMenu = false;
        FlashcardInput input = new();
        int stackId = input.ChooseStack();
        while(!rtnMainMenu)
        {
            Console.WriteLine("\n\nFLASHCARD MENU");
            Console.WriteLine("\nChoose from the options below");
            Console.WriteLine("Enter 1 - Add new Flashcard");
            Console.WriteLine("Enter 2 - View Flashcard stack");
            Console.WriteLine("Enter 3 - Alter existing Flashcard");
            Console.WriteLine("Enter 4 - Delete Flashcard");
            Console.WriteLine("Enter 5 - Change stack");
            Console.WriteLine("Enter 0 - Return to main menu");
            string flashcardInput = Console.ReadLine();

            while(string.IsNullOrEmpty(flashcardInput))
            {
                Console.WriteLine("Invalid entry. Please try again");
                flashcardInput = Console.ReadLine();
            }

            switch(flashcardInput)
            {
                case "1":
                    input.AddNewFlashcard(stackId);
                    break;
                case "2":
                    input.ViewFlashcards(stackId);
                    break;
                case "3":
                    input.ChangeFlashcard(stackId);
                    break;
                case "4":
                    input.DeleteFlashcard(stackId);
                    break;
                case "5":
                    stackId = input.ChangeStack();
                    break;
                case "0":
                    Console.Clear();
                    MainMenu mainMenu = new();
                    mainMenu.DisplayMenu();
                    break;
                default :
                    Console.WriteLine("Invalid entry. Please try again.");
                    break;
            }
        }
        
    }
}