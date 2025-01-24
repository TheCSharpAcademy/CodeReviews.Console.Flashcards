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



    }
}