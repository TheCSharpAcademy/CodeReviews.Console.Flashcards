using Flashcards.Controllers;

namespace Flashcards
{
    internal static class UserInput
    {

        internal static string GetStringInput(string message)
        {
            string input;
            Console.WriteLine(message);
            do
            {
                input = Console.ReadLine();
                if (input == "")
                {
                    Console.WriteLine("Input cannot be empty! Try again: ");
                }
            }
            while (input == "");
            return input;
        }

        internal static int GetIntInput(string message="")
        {
            int input;
            Console.WriteLine(message);
            while (!int.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("Invalid input! Try again: ");
            }
            return input;
        }

        internal static int GetCorrectStackId(CardStackController cardStackController, int id)
        {
            while (!cardStackController.StackExists(id))
            {
                id = UserInput.GetIntInput("Stack with this ID does not exist! Try again: ");
            }
            return id;
        }

        internal static (string, string) GetFlashcardInput(bool edit = false)
        {
            string term = UserInput.GetStringInput($"Enter the{(edit ? " new" : "")} term: ");
            string definition = UserInput.GetStringInput($"Enter the{(edit ? " new" : "")} definition: ");
            return (term, definition);
        }

        internal static int GetFlashcardId(FlashcardController flashcardController, int stackId)
        {
            int Id = UserInput.GetIntInput();
            while (Id > flashcardController.GetFlashcardCount(stackId))
            {
                Id = UserInput.GetIntInput("Flashcard with this ID does not exist! Try again: ");
            }
            return Id;
        }
    }
}