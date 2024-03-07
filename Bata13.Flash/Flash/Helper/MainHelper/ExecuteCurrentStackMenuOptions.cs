using Flash.Launching.SubManageStacks;
using Flash.Launching;

namespace Flash.Helper.MainHelper;

internal class ExecuteCurrentStackMenuOptions
{
    internal static void GetExecuteCurrentStackMenuOptions(string command, string currentWorkingStack)
    {
        switch (command)
        {
            case "0":
                Console.WriteLine("\nGoodbye!\n");
                MainMenu.GetMainMenu();
                break;
            case "1":
                Console.WriteLine("Change Current Stack");
                ManageStacks.GetManageStacks();
                break;
            case "2":
                Console.WriteLine("View All Flashcards in Stack");
                ViewAllFlashcards.GetViewAllFlashcards(currentWorkingStack);
                break;
            case "3":
                Console.WriteLine("View X amount of cards in stack");
                ViewXAmountFlashcards.GetViewXAmountFlashcards(currentWorkingStack);
                break;
            case "4":
                Console.WriteLine("Create a Flashcard in current stack");
                CreateFlashcard.GetCreateFlashcard(currentWorkingStack);
                break;
            case "5":
                Console.WriteLine("Edit a Flashcard");
                EditFlashcards.GetEditFlashcards(currentWorkingStack);
                break;
            case "6":
                Console.WriteLine("Delete a Flashcard");
                DeleteFlashcards.GetDeleteFlashcards(currentWorkingStack);
                break;

            default:
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                break;
        }
    }
}
