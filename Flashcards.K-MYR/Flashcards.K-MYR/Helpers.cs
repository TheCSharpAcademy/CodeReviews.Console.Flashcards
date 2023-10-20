using ConsoleTableExt;
using Flashcards.K_MYR.Models;
using System.Diagnostics;

namespace Flashcards.K_MYR;

internal static class Helpers
{
    internal static int PrintMenu(string[] options)
    {
        bool pressedEnter = false;
        int selected = 0;

        Console.WriteLine("-----------------------------");
        Console.WriteLine("|                           |");
        Console.WriteLine("|                           |");
        Console.WriteLine("|                           |");
        Console.WriteLine("|                           |");
        Console.WriteLine("|                           |");
        Console.WriteLine("-----------------------------");

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
        PrintStacks(tableData);
        Console.WriteLine("\n-------------------------------------------------------------------------------------------------------------------");
        Console.WriteLine("| Enter - View Flashcards | A - Add Stack | D - Delete Selected | R - Rename Selected | Esc - Return To Main Menu |");
        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
    }

    internal static void PrintStackOptions(List<CardStackDTO> tableData)
    {
        Console.Clear();
        Helpers.PrintStacks(tableData);
        Console.WriteLine("\n------------------------------------------------------------");
        Console.WriteLine("| Enter - Study Selected Stack | Esc - Return To Main Menu |");
        Console.WriteLine("------------------------------------------------------------");
    }

    internal static void PrintStacks(List<CardStackDTO> tableData)
    {
        ConsoleTableBuilder
            .From(tableData)
            .WithTitle("Flashcard Stacks", ConsoleColor.Green, ConsoleColor.Black)
            .WithTextAlignment(new Dictionary<int, TextAligntment>
                {
                    {0, TextAligntment.Center },
                    {1, TextAligntment.Center },
                    {2, TextAligntment.Center },
                })
            .WithColumn("", "Name", "# of Cards", "Created")
            .ExportAndWriteLine();
    }

    internal static void PrintCardsMenu(List<FlashcardDTO> tableData, string stackName)
    {
        Console.Clear();
        tableData.Sort((x, y) => x.Row - y.Row);
        Helpers.PrintFlashcards(tableData, stackName);
        Console.WriteLine("\n------------------------------------------------------------------------------------------------------------------");
        Console.WriteLine("| A - Add Flashcard | D - Delete Selected | F - Edit Front Text | B - Edit Back Text | Esc - Return To Main Menu |");
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
    }

    internal static void PrintFlashcards(List<FlashcardDTO> tableData, string stackName)
    {
        ConsoleTableBuilder
            .From(tableData)
            .WithTitle(stackName, ConsoleColor.Green, ConsoleColor.Black)
            .WithTextAlignment(new Dictionary<int, TextAligntment>
                {
                    {0, TextAligntment.Center },
                    {1, TextAligntment.Center },
                    {2, TextAligntment.Center },
                    {3, TextAligntment.Center },
                })
            .WithColumn("", "Front", "Back", "Created")
            .ExportAndWriteLine();
    }

    internal static void PrintSessions(List<SessionDTO> tableData)
    {
        ConsoleTableBuilder
            .From(tableData)
            .WithTitle("Study Sessions", ConsoleColor.Green, ConsoleColor.Black)
            .WithTextAlignment(new Dictionary<int, TextAligntment>
                {
                    {0, TextAligntment.Center },
                    {1, TextAligntment.Center },
                    {2, TextAligntment.Center },
                })
            .WithColumn("", "Stack", "Date", "Score", "Duration")
            .ExportAndWriteLine();
    }

    internal static void PrintReport(List<ReportEntryScore> sumScore, List<ReportEntryScore> avgScore,
       List<ReportEntryTime> sumTime, List<ReportEntryTime> avgTime, string title)
    {
        Console.Clear();
        Console.WriteLine("-----------------------------");
        Console.WriteLine("| Please enter a year: YYYY |");
        Console.WriteLine("-----------------------------\n");

        ConsoleTableBuilder
            .From(sumScore)
            .WithTitle($"Score Per Month For {title}", ConsoleColor.Green, ConsoleColor.Black)
            .WithColumn("Stack", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December")
            .WithTextAlignment(new Dictionary<int, TextAligntment>
            {
                {0, TextAligntment.Center },
                {1, TextAligntment.Center },
                {2, TextAligntment.Center },
                {3, TextAligntment.Center },
                {4, TextAligntment.Center },
                {5, TextAligntment.Center },
                {6, TextAligntment.Center },
                {7, TextAligntment.Center },
                {8, TextAligntment.Center },
                {9, TextAligntment.Center },
                {10, TextAligntment.Center },
                {11, TextAligntment.Center },
                {12, TextAligntment.Center },
            })
            .WithMinLength(new Dictionary<int, int>
            {
                { 1, 8 },
                { 2, 8 },
                { 3, 8 },
                { 4, 8 },
                { 5, 8 },
                { 6, 8 },
                { 7, 8 },
                { 8, 8 },
                { 9, 8 },
                { 10, 8 },
                { 11, 8 },
                { 12, 8 },
            })
            .ExportAndWriteLine();
        Console.WriteLine();

        ConsoleTableBuilder
           .From(avgScore)
           .WithTitle($"Average Score Per Month For {title}", ConsoleColor.Green, ConsoleColor.Black)
           .WithColumn("Stack", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December")
           .WithTextAlignment(new Dictionary<int, TextAligntment>
           {
                {0, TextAligntment.Center },
                {1, TextAligntment.Center },
                {2, TextAligntment.Center },
                {3, TextAligntment.Center },
                {4, TextAligntment.Center },
                {5, TextAligntment.Center },
                {6, TextAligntment.Center },
                {7, TextAligntment.Center },
                {8, TextAligntment.Center },
                {9, TextAligntment.Center },
                {10, TextAligntment.Center },
                {11, TextAligntment.Center },
                {12, TextAligntment.Center },
           })
           .WithMinLength(new Dictionary<int, int>
           {
                { 1, 8 },
                { 2, 8 },
                { 3, 8 },
                { 4, 8 },
                { 5, 8 },
                { 6, 8 },
                { 7, 8 },
                { 8, 8 },
                { 9, 8 },
                { 10, 8 },
                { 11, 8 },
                { 12, 8 },
           })
           .ExportAndWriteLine();
        Console.WriteLine();

        ConsoleTableBuilder
           .From(sumTime)
           .WithTitle($"Time Per Month For {title}", ConsoleColor.Green, ConsoleColor.Black)
           .WithColumn("Stack", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December")
           .WithTextAlignment(new Dictionary<int, TextAligntment>
           {
                {0, TextAligntment.Center },
                {1, TextAligntment.Center },
                {2, TextAligntment.Center },
                {3, TextAligntment.Center },
                {4, TextAligntment.Center },
                {5, TextAligntment.Center },
                {6, TextAligntment.Center },
                {7, TextAligntment.Center },
                {8, TextAligntment.Center },
                {9, TextAligntment.Center },
                {10, TextAligntment.Center },
                {11, TextAligntment.Center },
                {12, TextAligntment.Center },
           })
           .WithMinLength(new Dictionary<int, int>
           {
                { 1, 8 },
                { 2, 8 },
                { 3, 8 },
                { 4, 8 },
                { 5, 8 },
                { 6, 8 },
                { 7, 8 },
                { 8, 8 },
                { 9, 8 },
                { 10, 8 },
                { 11, 8 },
                { 12, 8 },
           })
           .ExportAndWriteLine();
        Console.WriteLine();

        ConsoleTableBuilder
           .From(avgTime)
           .WithTitle($"Average Time Per Month For {title}", ConsoleColor.Green, ConsoleColor.Black)
           .WithColumn("Stack", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December")
           .WithTextAlignment(new Dictionary<int, TextAligntment>
           {
                {0, TextAligntment.Center },
                {1, TextAligntment.Center },
                {2, TextAligntment.Center },
                {3, TextAligntment.Center },
                {4, TextAligntment.Center },
                {5, TextAligntment.Center },
                {6, TextAligntment.Center },
                {7, TextAligntment.Center },
                {8, TextAligntment.Center },
                {9, TextAligntment.Center },
                {10, TextAligntment.Center },
                {11, TextAligntment.Center },
                {12, TextAligntment.Center },
           })
           .WithMinLength(new Dictionary<int, int>
           {
                { 1, 8 },
                { 2, 8 },
                { 3, 8 },
                { 4, 8 },
                { 5, 8 },
                { 6, 8 },
                { 7, 8 },
                { 8, 8 },
                { 9, 8 },
                { 10, 8 },
                { 11, 8 },
                { 12, 8 },
           })
           .ExportAndWriteLine();
        Console.WriteLine();

        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine("| Enter - Submit Year | Esc - Return To Main Menu |");
        Console.WriteLine("---------------------------------------------------");
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        Random random = new();
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    public static void Count(CancellationToken token)
    {
        Stopwatch sw = Stopwatch.StartNew();

        while (!token.IsCancellationRequested)
        {
            Thread.Sleep(1000);
            var originalX = Console.CursorLeft;
            var originalY = Console.CursorTop;

            Console.SetCursorPosition(50, 1);
            Console.Write(sw.Elapsed.ToString("hh\\:mm\\:ss"));

            Console.SetCursorPosition(originalX, originalY);
        }
    }

    public static void ReassignRows<T>(IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].GetType().GetProperty("Row").SetValue(list[i], i + 1);
        }
    }
}
