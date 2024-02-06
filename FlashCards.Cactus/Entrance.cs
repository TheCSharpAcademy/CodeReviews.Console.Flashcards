using ConsoleTableExt;
using FlashCards.Cactus;

namespace FlashCards;
public class Entrance
{
    public static void Main(string[] args)
    {
        Application app = new Application();

        while (true)
        {
            PrintMenu();
            string? op = Console.ReadLine();
            switch (op)
            {
                case "0":
                    Environment.Exit(0);
                    break;
                case "1":
                    app.ManageStacks();
                    break;
                default:
                    break;
            }
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