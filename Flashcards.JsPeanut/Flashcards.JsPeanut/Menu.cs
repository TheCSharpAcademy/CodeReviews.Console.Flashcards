using System.Text;

namespace TestingArea
{
    internal class Menu
    {
        public enum MenuDirection
        {
            Horizontal,
            Vertical
        }
        List<ConsoleKey> keys = new();
        public static int ShowMenu(List<ConsoleKey> keys, string welcomeMsg = "", string arrowstouseMsg = "", params string[] opts)
        {
            
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (!string.IsNullOrEmpty(welcomeMsg))
            {
                Console.WriteLine($"{welcomeMsg}");
            }
            Console.ResetColor();
            if (!string.IsNullOrEmpty(arrowstouseMsg))
            {
                Console.WriteLine($"{arrowstouseMsg}");
            }
            (int left, int top) = Console.GetCursorPosition();
            var option = 0;
            var decorator = $"[ \u001b[32m";
            ConsoleKeyInfo key;
            bool isSelected = false;

            while (!isSelected)
            {
                Console.SetCursorPosition(left, top);

                var firstKey = keys[0];

                var secondKey = keys[1];

                var notNullOrEmptyOpts = opts.Where(opt => !string.IsNullOrEmpty(opt)).ToArray();

                opts = notNullOrEmptyOpts;

                for (int i = 0; i < opts.Length; i++)
                {
                    if (firstKey == ConsoleKey.UpArrow && secondKey == ConsoleKey.DownArrow)
                    {
                        Console.WriteLine($"{(option == i ? decorator : "  ")}{opts[i]}\u001b[0m");
                    }
                    else
                    {
                        Console.Write($"{(option == i ? decorator : "  ")}{opts[i]}\u001b[0m");
                    }
                }

                key = Console.ReadKey(false);

                switch (key.Key)
                {
                    //Reason I added a when statement: If a user had to navigate through one of the menus with UpArrow and DownArrow, but pressed instead Left and RightArrow, those two would work and they shouldn't.
                    case ConsoleKey.UpArrow when firstKey == ConsoleKey.UpArrow:
                        option = option == 0 ? opts.Length - 1 : option - 1;
                        break;
                    case ConsoleKey.DownArrow when secondKey == ConsoleKey.DownArrow:
                        option = option == opts.Length - 1 ? 0 : option + 1;
                        break;
                    case ConsoleKey.LeftArrow when firstKey == ConsoleKey.LeftArrow:
                        option = option == 0 ? opts.Length - 1 : option - 1;
                        break;
                    case ConsoleKey.RightArrow when secondKey == ConsoleKey.RightArrow:
                        option = option == opts.Length - 1 ? 0 : option + 1;
                        break;
                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                }
            }
            Console.WriteLine($"\n{decorator}You selected Option {option + 1}");
            Console.ResetColor();
            return option;
        }
    }
}