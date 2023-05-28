namespace Flashcards
{
    internal class UserInput
    {
        public static string GetUserOption()
        {
            string input = Console.ReadLine();
            while (!Validator.IsValidOption(input))
            {
                Console.Write("\nThis is not a valid input. Please enter one of the above options: ");
                input = Console.ReadLine();
            }
            return input;
        }

        internal static int GetIndex<T>(List<T> list)
        {
            while (true)
            {
                int result;
                if (Validator.IsInteger(out result))
                {
                    if (Validator.IsIntegerInListOrExit(list, result))
                    {
                        return result;
                    }
                }
                Console.Write("\nThis is not a valid id, please enter a number or to return to main menu type '-1': ");
            }
        }

        public static string ChooseStackOrFlashcard()
        {
            string input = Console.ReadLine();
            while (!Validator.IsStackOrFlashcard(input))
            {
                Console.Write("\nThis is not a valid input. Please enter one of the above options: ");
                input = Console.ReadLine();
            }
            return input;
        }

        public static string GetDbFriendlyString()
        {
            string input = Console.ReadLine();
            while (String.IsNullOrEmpty(input) || input.Length > 255)
            {
                Console.WriteLine("\nInput cannot be empty or more than 255 characters");
                input = Console.ReadLine();
            }
            return input;
        }
    }
}
