using ConsoleTableExt;
using DataAccess.Models;

namespace Flashcards.SamGannon.UI;

internal class TableVisualization
{
    internal static void ShowTable<T>(List<T> tableData, string tableTitle) where T : class
    {
        Console.Clear();
        Console.WriteLine("\n\n");

        ConsoleTableBuilder
            .From(tableData)
            .WithTitle(tableTitle)
            .ExportAndWriteLine();
        Console.Write("\n\n");
        
    }
}
