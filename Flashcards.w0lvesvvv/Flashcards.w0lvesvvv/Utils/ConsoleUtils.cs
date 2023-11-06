namespace Flashcards.w0lvesvvv.Utils
{
    public static class ConsoleUtils
    {
        public static void SetColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        public static void DisplayMessage(string message, bool expectInput = false, bool inline = false, ConsoleColor messageColor = ConsoleColor.Green, ConsoleColor inputColor = ConsoleColor.White)
        {
            SetColor(messageColor);
            if (inline) { Console.Write(message); } else { Console.WriteLine(message); }
            if (expectInput) SetColor(inputColor);
        }
    }
}
