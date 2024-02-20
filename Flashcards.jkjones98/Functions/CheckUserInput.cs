using Flashcards.jkjones98;

namespace Flashcards.Functions;

internal class CheckUserInput
{
    FlashcardController controller = new();
    StackController stackController = new();
    internal int ChooseStack()
    {
        Console.Clear();
        stackController.ViewStackDb();
        Console.WriteLine("\nEnter the Id of the stack you would like to enter?");
        string stackChoice = Console.ReadLine();
        int stackId = CheckForChar(stackChoice,"Stacks","StackId");;

        Console.WriteLine("\n\nStack found!");
        return stackId;
    }
    internal int CheckForChar()
    {
        string userInput = Console.ReadLine();
        int parsedInput;
        while(string.IsNullOrEmpty(userInput) || userInput.Any(char.IsLetter) || !Int32.TryParse(userInput, out parsedInput))
        {

            Console.WriteLine("Invalid input. Please enter a valid Id.");
            userInput = Console.ReadLine();
        }
        return parsedInput;
    }
    internal int CheckForChar(string userInput, string tblName, string prmryKey)
    {
        int parsedInput;
        while(string.IsNullOrEmpty(userInput) || userInput.Any(char.IsLetter) || !Int32.TryParse(userInput, out parsedInput) || !controller.CheckIdExists(parsedInput,tblName,prmryKey))
        {

            Console.WriteLine("Invalid input. Please enter a valid Id.");
            userInput = Console.ReadLine();
        }
        return parsedInput;
    }
    internal int CheckForChar(string userInput, string tblName, string prmryKey, int stackId)
    {
        int parsedInput;
        while(string.IsNullOrEmpty(userInput) || userInput.Any(char.IsLetter) || !Int32.TryParse(userInput, out parsedInput) || !controller.CheckIdExists(parsedInput,tblName,prmryKey, stackId))
        {

            Console.WriteLine("Invalid input. Please enter a valid Id.");
            userInput = Console.ReadLine();
        }
        return parsedInput;
    }

    internal string CheckForDigit()
    {
        string userInput = Console.ReadLine();
        
        while(string.IsNullOrEmpty(userInput) || userInput.Any(char.IsDigit))
        {
            Console.WriteLine("Empty answer, or word contains a digit. Please enter again.");
            userInput = Console.ReadLine();
        }

        return userInput;
    }
}