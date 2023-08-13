using ConsoleTableExt;

using Flashcards.MartinL_no.Models;

namespace Flashcards.MartinL_no.UserInterface;

internal static class TableVisualizationEngine
{
    public static void ShowTable(List<string> stacks)
    {
        var tableData = FormatTableData(stacks);
        BuildTable(tableData, new string[] { "Name" });
    }

    public static void ShowTable(FlashcardStackDTO stack)
    {
        var tableData = FormatTableData(stack);
        BuildTableWithTile(tableData, stack.Name, new string[] { "Id", "Front", "Back" });
    }

    private static void BuildTable(List<List<object>> tableData, params string[] columnNames)
    {
        ConsoleTableBuilder
            .From(tableData)
            .WithColumn(columnNames)
            .ExportAndWriteLine();
        Console.WriteLine();
    }

    private static void BuildTableWithTile(List<List<object>> tableData, string title, params string[] columnNames)
    {
        ConsoleTableBuilder
            .From(tableData)
            .WithTitle(title)
            .WithColumn(columnNames)
            .ExportAndWriteLine();
    }

    private static List<List<object>> FormatTableData(List<string> stacks)
    {
        return stacks.Select(s => new List<object>
            {
                s,
            }).ToList();
    }

    private static List<List<object>> FormatTableData(FlashcardStackDTO stack)
    {
        return stack.Flashcards.Select(s => new List<object>
            {
                s.ViewId,
                s.Original,
                s.Translation,
            }).ToList();
    }
}
