using System;
using Spectre.Console;

namespace FlashCard.SheheryarRaza
{
    public static class ConsoleHelper
    {
        public static string GetStringInput(string prompt)
        {
            return AnsiConsole.Ask<string>($"[white]{prompt}[/]");
        }

        public static int GetIntInput(string prompt)
        {
            return AnsiConsole.Ask<int>($"[white]{prompt}[/]");
        }

        public static void DisplayMessage(string message, ConsoleColor color = ConsoleColor.White)
        {
            string spectreColor = color switch
            {
                ConsoleColor.Red => "red",
                ConsoleColor.Green => "green",
                ConsoleColor.Yellow => "yellow",
                ConsoleColor.Cyan => "cyan",
                ConsoleColor.DarkYellow => "darkyellow",
                _ => "white"
            };
            AnsiConsole.MarkupLine($"[{spectreColor}]{message}[/]");
        }

        public static void ClearConsole()
        {
            AnsiConsole.Clear();
        }

        public static void PressAnyKeyToContinue()
        {
            AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
            Console.ReadKey();
        }
    }
}
