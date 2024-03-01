using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTableExt;
using DataAccess.Models;

namespace Flashcards.SamGannon.UI
{
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

        internal static void ShowSingleRow<T>(List<T> tableData, string tableTitle) where T : class
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
}
