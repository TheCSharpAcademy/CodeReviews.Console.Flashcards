using Spectre.Console;
using static System.Console;

namespace Flashcards.nikosnick13
{
    internal class Validation
    {
        public static bool isValidString(string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                WriteLine("The input is empty or null.");
                return false;
            }
            if (str.Length < 3 || str.Length > 255) 
            {
                WriteLine();
                return false;
            }
            if (str.Any(char.IsDigit))
            {
                WriteLine("The input must not contain numbers.");
                return false;
            }
            return true;
        }

        public static bool isValidInt(string? userInput)
        {
            if (string.IsNullOrEmpty(userInput)) return false;

            return int.TryParse(userInput, out int result) && result > 0;
        }

        public static bool ConfirmEdit(string msg) 
        {

            var confirmation = AnsiConsole.Prompt(
                new TextPrompt<bool>(msg)
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(true)
                .WithConverter(choice => choice ? "y" : "n")
                );
            return confirmation;

        }

        public static int GetValidNumber(string prompt, int minValue, int maxValue)
        {
            int userInput;
            while (true)
            {
                // Show the prompt and get input
                Console.Write(prompt);
                string input = Console.ReadLine();

                // Try parsing the input as an integer
                if (int.TryParse(input, out userInput))
                {
                    // Check if the number is within the specified range
                    if (userInput >= minValue && userInput <= maxValue)
                    {
                        return userInput;
                    }
                    else
                    {
                        // Inform the user that the number is out of range
                        Console.WriteLine($"Please enter a number between {minValue} and {maxValue}.");
                    }
                }
                else
                {
                    // Inform the user that the input is not a valid number
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }
        }
    }
}