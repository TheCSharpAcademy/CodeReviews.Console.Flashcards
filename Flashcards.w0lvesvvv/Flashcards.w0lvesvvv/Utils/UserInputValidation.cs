namespace Flashcards.w0lvesvvv.Utils
{
    public static class UserInputValidation
    {
        public static bool ValidateNumber(string number, out int parsedNumber)
        {
            if (int.TryParse(number, out parsedNumber))
            {
                return true;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid input.");
            return false;
        }
    }
}
