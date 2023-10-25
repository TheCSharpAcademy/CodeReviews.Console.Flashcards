using ConsoleTableExt;

namespace CodingTrackerConsoleUI
{
    public class TableVisualisation
    {
        public static void ShowTable<T>(List<T> tableData) where T :class
        {
            Console.WriteLine("\n\n");
            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Stacks")
                .ExportAndWriteLine();
            Console.WriteLine("\n\n");
        }
    }
}