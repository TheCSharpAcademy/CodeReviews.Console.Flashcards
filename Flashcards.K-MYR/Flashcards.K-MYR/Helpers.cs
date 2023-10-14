using ConsoleTableExt;
using Flashcards.K_MYR.Models;
using System.Collections.Generic;

namespace Flashcards.K_MYR;

internal class Helpers
{
    internal static int PrintMenu(string[] options)
    {
        bool pressedEnter = false;
        int selected = 0;        
        Console.WriteLine("----------------------------");
        Console.WriteLine("|                          |");
        Console.WriteLine("|                          |");
        Console.WriteLine("|                          |");
        Console.WriteLine("|                          |");
        Console.WriteLine("|                          |");
        Console.WriteLine("----------------------------");

        Console.CursorTop = 1;         

        while (!pressedEnter)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.CursorLeft = 1;

                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("> ");
                }
                else
                {
                    Console.Write(" ");
                }

                Console.WriteLine(options[i]);
                Console.ResetColor();
            }

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:
                    selected = Math.Max(0, selected - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selected = Math.Min(options.Length - 1, selected + 1);
                    break;
                case ConsoleKey.Enter:
                    pressedEnter = true;
                    break;
            }

            if (!pressedEnter)
                Console.CursorTop -= options.Length;
        }

        return selected;
    }

    internal static void PrintStacksMenu(List<CardStackDTO> tableData)
    {
        Console.Clear();
        tableData.Sort((x,y) => x.Name.CompareTo(y.Name));
        Helpers.PrintStacks(tableData);
        Console.WriteLine("\n---------------------------------------------------------------------------------------");
        Console.WriteLine("| A - Add Stack | D - Delete Selected | R - Rename Selected | E - Return To Main Menu |");
        Console.WriteLine("---------------------------------------------------------------------------------------");
    }

    internal static void PrintStacks(List<CardStackDTO> tableData)
    {
       
        ConsoleTableBuilder
            .From(tableData)
            .WithTitle("Flashcard Stacks", ConsoleColor.Green, ConsoleColor.Black)
            .WithTextAlignment(new Dictionary<int, TextAligntment>
                {
                    {1, TextAligntment.Center },
                    {2, TextAligntment.Center },
                    {3, TextAligntment.Center },
                })
            .WithColumn("Name", "# of Cards", "Created")
            .ExportAndWriteLine();
    }
}
