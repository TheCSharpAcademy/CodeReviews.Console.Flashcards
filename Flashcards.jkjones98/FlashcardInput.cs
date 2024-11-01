using Flashcards.Functions;

namespace Flashcards.jkjones98;

internal class FlashcardInput
{
    CheckUserInput checkUserInput = new();
    StackController stackController = new();
    FlashcardController controller = new();
    internal void AddNewFlashcard(int stackId)
    {
        controller.ViewFlashcardDto(stackId);
        Console.WriteLine("\n\nPlease enter the word you like to memorize below");
        string frontWord = checkUserInput.CheckForDigit();

        Console.WriteLine("\nPlease enter the meaning of this word in your language");
        string meaning = checkUserInput.CheckForDigit();

        Flashcard flashcard = new()
        {
            Front = frontWord,
            Back = meaning,
            StackId = stackId
        };
        controller.InsertFlashcardDb(flashcard);
    }

    internal void ViewFlashcards(int stackId)
    {
        controller.ViewFlashcardDto(stackId);
    }

    internal void ChangeFlashcard(int stackId)
    {
        string columnName;
        int flashId;

        controller.ViewFlashcardDb(stackId, "StackId");
        Console.WriteLine("Which Flashcard would you like to change?");
        string cardId = Console.ReadLine();
        flashId = checkUserInput.CheckForChar(cardId,"Flashcards","FlashId", stackId);

        Console.WriteLine("\n\nWould you like to change the Front of the card, the Back of the card or the Stack this belongs to?");
        Console.WriteLine("Enter F for Front");
        Console.WriteLine("Enter B for Back");
        Console.WriteLine("Enter S for Stack");
        string choice = checkUserInput.CheckForDigit();

        choice = choice.ToUpper();

        switch(choice)
        {
            case "F":
                controller.ViewFlashcardDb(flashId, "FlashId");
                columnName = "Front";
                Console.WriteLine("\nEnter what you would like this to change the Front to");
                string front = checkUserInput.CheckForDigit();
                controller.ChangeFlashcardDb(flashId, columnName, front, stackId);
                break;
            case "B":
                controller.ViewFlashcardDb(flashId, "FlashId");
                columnName = "Back";
                Console.WriteLine("\nEnter what you would like this to change the Back to");
                string back = checkUserInput.CheckForDigit();
                controller.ChangeFlashcardDb(flashId, columnName, back, stackId);
                break;
            case "S":
                Console.Clear();
                controller.ViewFlashcardDb(flashId, "StackId");
                stackController.ViewStackDb(stackId);
                columnName = "StackId";
                Console.WriteLine("\nEnter what you would like this to change the Stack to");
                string stack = Console.ReadLine();
                int parseStack = checkUserInput.CheckForChar(stack,"Stacks","StackId");
 
                controller.ChangeFlashcardDb(flashId, columnName, parseStack, stackId);
                break;
            default:
                Console.WriteLine("Invalid option selected please try again");
                ChangeFlashcard(stackId);
                break;
        }

    }

    internal void DeleteFlashcard(int stackId)
    {
        Console.Clear();
        controller.ViewFlashcardDb(stackId, "StackId");
        Console.WriteLine("\n\nPlease enter the Id of the flashcard you would like to delete");
        string cardId = Console.ReadLine();
        int deleteId = checkUserInput.CheckForChar(cardId,"Flashcards","FlashId", stackId);

        Console.WriteLine("Flashcard removed!");

        controller.DeleteFlashcardDb(deleteId);
    }
}