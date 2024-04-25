namespace Flashcards
{
    class Validation
    {
        FlashcardsController flashcardsController = new();
        StacksController stacksController = new();
        internal static GetUserInput getUserInput = new GetUserInput();
        internal void ToMainMenu(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Press enter to go back to main menu.");
            Console.ReadLine();
            getUserInput.MainMenu();
        }

        internal int LessThanOne(string message, int stackId, int number)
        {
            if (number == 0)
            {
                Console.Clear();
                Console.WriteLine($"Number  entered is less than zero.");
                stacksController.ViewStacks();
                stackId = getUserInput.GetNumInput(message, "stacks");
                return stackId;
            }
            if (number == 1)
            {
                int flashcardId;
                Console.Clear();
                Console.WriteLine($"Number entered is less than zero.");
                flashcardsController.ReadFlashcards(stackId);
                flashcardId = getUserInput.GetNumInput(message, "flashcards", stackId);
                return flashcardId;
            }
            return number;
        }

        internal int BiggerThanList(string message,int stackId, int number)
        {
            if (number == 0)
            {
                Console.Clear();
                Console.WriteLine("Number entered is bigger than the amount of records.");
                stacksController.ViewStacks();
                stackId = getUserInput.GetNumInput(message, "stacks");
                return stackId;
            }
            else if (number == 1)
            {
                int flashcardId;
                Console.Clear();
                Console.WriteLine("Number entered is bigger than the amount of records.");
                flashcardsController.ReadFlashcards(stackId);
                flashcardId = getUserInput.GetNumInput(message, "flashcards", stackId);
                return flashcardId;
            }
            return number;
        }
    }
}