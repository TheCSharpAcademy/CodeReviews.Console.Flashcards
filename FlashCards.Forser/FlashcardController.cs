namespace FlashCards.Forser
{
    internal class FlashcardController
    {
        internal void ShowFlashcardMenu()
        {
            StackController stackController = new StackController();
            MainMenuController mainMenuController = new MainMenuController();

            Console.Clear();
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("              FLASHCARD MENU");
            Console.WriteLine("Select - Select a Stack");
            Console.WriteLine("List - List all Flashcards from selected Stack");
            Console.WriteLine("Add - Add a new FlashCard/s to selected Stack");
            Console.WriteLine("Edit - Edit FlashCard/s from selected Stack");
            Console.WriteLine("Delete - Delete FlashCard/s from selected Stack\n");
            Console.WriteLine("Stack - Go to Stack Menu");
            Console.WriteLine("Menu - Return to Main Menu");
            Console.WriteLine("------------------------------------------");

            string selectedFlashcardMenu = Console.ReadLine().Trim().ToLower();

            switch (selectedFlashcardMenu)
            {
                case "select":
                    break;
                case "list":
                    break;
                case "add":
                    break;
                case "edit":
                    break;
                case "delete":
                    break;
                case "menu":
                    mainMenuController.MainMenu();
                    break;
                case "stack":
                    stackController.ShowStackMenu();
                    break;
                default:
                    Console.WriteLine("Not a valid option, select from an option from the Menu");
                    break;
            }
        }
    }
}