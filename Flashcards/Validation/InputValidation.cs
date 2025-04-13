using Flashcards.Model;

namespace Flashcards.Validation
{
    public static class InputValidation
    {
        public static bool IsValidString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty or whitespace.");
                return false;
            }
            return true;
        }

        public static int GetQuantity(string message)
        {
            Console.WriteLine(message);

            string quantityInput = Console.ReadLine();

            if (quantityInput == "0") UserInputManager.Menu();

            while (!Int32.TryParse(quantityInput, out _))
            {
                Console.WriteLine("Invalid input. Please try again.");
                quantityInput = Console.ReadLine();
            }

            return Convert.ToInt32(quantityInput);
        }

        public static bool IsValidDate(string input)
        {
            if (!DateTime.TryParse(input, out _))
            {
                Console.WriteLine("Input must be a valid date.");
                return false;
            }
            return true;
        }
    }
}
