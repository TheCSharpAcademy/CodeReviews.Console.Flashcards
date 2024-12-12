using System.Globalization;

namespace flashcard_app
{
    /// <summary>
    /// Responsible for taking in all user input and validating it
    /// </summary>
    class UserInput
    {
        internal static string? GetStringInput(string message)
        {
            Console.WriteLine(message);

            string? stringInput = Console.ReadLine();

            if (stringInput == "0") return null;

            while (stringInput == null)
            {
                Console.WriteLine("\n\nInvalid input, please try again:");
                stringInput = Console.ReadLine();
            }

            return stringInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string? numberInput = Console.ReadLine();

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number. Try again.\n\n");
                numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }
    }
}