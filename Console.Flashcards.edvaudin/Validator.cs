using Flashcards.Models;

namespace Flashcards
{
    internal class Validator
    {
        public static bool IsValidOption(string input)
        {
            string[] validOptions = { "s", "v", "a", "d", "u", "0" };
            foreach (string validOption in validOptions)
            {
                if (input == validOption)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsValidStackId(int input)
        {
            DataAccessor dal = new();
            StackDTO stack = dal.GetStackById(input);
            return stack != null;
        }

        public static bool IsStackOrFlashcard(string input)
        {
            string[] validOptions = { "s", "f" };
            foreach (string validOption in validOptions)
            {
                if (input == validOption)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
