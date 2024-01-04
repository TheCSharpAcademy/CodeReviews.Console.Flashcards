using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTableExt;
namespace FlashCards.Ibrahim.UI
{
    public class TableVisualization
    {
        public static void ShowTable<T>(List<T> history) where T : class
        {
            ConsoleTableBuilder
                .From(history)
                .ExportAndWriteLine();
        }
    }
}
