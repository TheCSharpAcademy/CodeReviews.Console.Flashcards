namespace Flashcards.w0lvesvvv.Utils
{
    public class UserInput
    {

        public static string RequestString(string message)
        {
            ConsoleUtils.DisplayMessage(message, true, true);
            return UserInput.ReadString();
        }

        public static int? RequestNumber(string message)
        {
            ConsoleUtils.DisplayMessage(message, true, true);
            return UserInput.ReadNumber();

        }

        public static int? ReadNumber()
        {
            string inputNumber = Console.ReadLine() ?? string.Empty;
            if (!UserInputValidation.ValidateNumber(inputNumber, out int parsedNumber)) return null;

            return parsedNumber;
        }

        public static string ReadString()
        {
            return Console.ReadLine() ?? string.Empty;
        }

    }
}
