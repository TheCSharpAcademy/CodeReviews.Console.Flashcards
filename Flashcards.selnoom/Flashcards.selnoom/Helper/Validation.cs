namespace Flashcards.selnoom.Helper;

internal static class Validation
{
    internal static int ValidateStringToInt(string userInput)
    {
        int validatedInput;
        while(!int.TryParse(userInput, out validatedInput))
        {
            Console.WriteLine("\nInvalid input. Please try again:");
            Console.ReadLine();
            userInput = Console.ReadLine();
        }
        return validatedInput;
    }

    internal static int GetValidatedId(string prompt, int min, int max)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string userInput = Console.ReadLine();
            int validatedInput = ValidateStringToInt(userInput);
            if (validatedInput == 0)
            {
                return 0;
            }
            if (validatedInput < min || validatedInput > max)
            {
                Console.WriteLine("The selected Id does not exist. Please try again.\n");
            }
            else
            {
                return validatedInput;
            }
        }
    }

    internal static string GetValidatedUniqueInput(string prompt, List<string> existingItems, string duplicateMessage)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine()?.Trim();
            if (input == "0")
            {
                return "0"; // Return "0" as a signal to exit
            }
            if (existingItems.Contains(input))
            {
                Console.WriteLine(duplicateMessage);
            }
            else if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input cannot be empty. Please try again.\n");
            }
            else
            {
                return input;
            }
        }
    }
}
