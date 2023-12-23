using ConsoleTableExt;

namespace Flashcards
{
    internal class TableVisualisationEngine
    {
        internal static void DrawTable(List<List<object>> tableData)
        {
            // https://github.com/minhhungit/ConsoleTableExt
            ConsoleTableBuilder
                .From(tableData)
                .ExportAndWriteLine();
        }
    }
}
