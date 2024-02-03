using ConsoleTableExt;

namespace FlashCards;
public class Entrance
{
    public static void Main(string[] args)
    {
        bool endApp = false;
        while (!endApp)
        {
            PrintMenu();
            string? op = Console.ReadLine();
            switch (op)
            {
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
            Console.Write("\nPress 'q' and Enter to close the app, or press any other key and Enter to return MAIN MENU. ");
            if (Console.ReadLine() == "q") endApp = true;
            Console.WriteLine("\n");
        }
    }

    private static void PrintMenu()
    {
        Console.Clear();

        List<string> menuData = new List<string>
            {
                "<0> Exit app.",
                "<1> Manage Stacks.",
                "<2> Manage FlashCards.",
                "<3> Study.",
                "<4> Study report."
            };

        ConsoleTableBuilder
        .From(menuData)
        .WithTitle("MAIN MENU", ConsoleColor.Yellow, ConsoleColor.DarkGray)
        .WithFormat(ConsoleTableBuilderFormat.Alternative)
        .ExportAndWriteLine(TableAligntment.Center);
    }
}