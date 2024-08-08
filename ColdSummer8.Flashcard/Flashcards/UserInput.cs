namespace Flashcards
{
    public class UserInput
    {
        public static int userInput;
        public static int NumericInputOnly()
        {
            string msg = string.Empty;
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    if (char.IsDigit(key.KeyChar) || key.KeyChar == '.')
                    {
                        if (key.KeyChar == '.' && msg.Contains(".")) continue;
                        else
                        {
                            msg += key.KeyChar;
                            Console.Write(key.KeyChar);
                        }
                    }
                }
                else if (key.Key == ConsoleKey.Backspace && msg.Length > 0)
                {
                    msg = msg.Substring(0, (msg.Length - 1));
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter || string.IsNullOrEmpty(msg));

            if (int.TryParse(msg, out int val))
            {
                return val;
            }
            return userInput;
        }
    }
}
