using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlashCards;

public static class UserInput
{
    public static void DisplayMessage(string message = "", string actionMessage = "continue", bool consoleClear = false)
    {
        if (consoleClear) Console.Clear();

        if (message == "")
            Console.WriteLine($"\nPress any key to {actionMessage}...");
        else
            Console.WriteLine($"\n{message} Press any key to {actionMessage}...");

        Console.ReadKey();
    }

    public static string InputWithSpecialKeys(MenuManager menuManager, bool escapeOption, int maximumChars = 100)
    {
        var userInput = new StringBuilder();
        ConsoleKeyInfo keyPress;

        do
        {
            keyPress = Console.ReadKey(true);

            if (keyPress.Key == ConsoleKey.Escape)
            {
                if (!escapeOption) continue;

                menuManager.GoBack();
            }

            else if (keyPress.Key == ConsoleKey.Enter)
            {
                Console.Write("\n");
                return userInput.ToString();
            }

            else if (keyPress.Key == ConsoleKey.Backspace && userInput.Length > 0)
            {
                Console.Write("\b \b");
                userInput.Length--;
            }

            else if (!char.IsControl(keyPress.KeyChar)&& userInput.Length < maximumChars)
            {
                Console.Write(keyPress.KeyChar);
                userInput.Append(keyPress.KeyChar);
            }
        } while (true);

    }
}
