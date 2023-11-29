using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTableExt;

namespace Flashcards.SamGannon.UI
{
    internal class TableVisualization
    {
        internal static void ShowTable<T>(List<T> tableData) where T : class
        {
            Console.Clear();
            Console.WriteLine("\n\n");

            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("=== Manage Stacks ===")
                .ExportAndWriteLine();
            Console.Write("\n\n");
            Console.ReadLine();
        }
    }
}
