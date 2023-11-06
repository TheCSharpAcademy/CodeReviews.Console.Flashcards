using ConsoleTableExt;

namespace Flashcards.w0lvesvvv.Utils
{
    public static class TableBuilder
    {
        public static void BuildTable<T>(List<T> table, ConsoleTableBuilderFormat format = ConsoleTableBuilderFormat.Alternative) where T : class
        {
            ConsoleTableBuilder.From(table)
                .WithFormat(format)
                .ExportAndWriteLine();
        }
    }
}
