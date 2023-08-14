using ConsoleTableExt;

using Flashcards.MartinL_no.Models;

namespace Flashcards.MartinL_no.UserInterface;

internal static class TableVisualizationEngine
{
    public static void ShowStackNameTable(List<string> stacks)
    {
        var tableData = FormatTableData(stacks);
        BuildTable(tableData, new string[] { "Name" });
    }

    public static void ShowFlashcardsTable(FlashcardStackDto stack)
    {
        var tableData = FormatTableData(stack);
        BuildTableWithTitle(tableData, stack.Name, new string[] { "Id", "Front", "Back" });
    }

    public static void ShowStudySessionsTable(List<StudySessionDto> sessions)
    {
        var tableData = FormatTableData(sessions);
        BuildTable(tableData, new string[] { "Stack", "Date", "Score" });
    }

    public static void ShowStudyQuestionTable(FlashcardDto flashcard, string stackName)
    {
        var tableData = FormatTableData(flashcard);
        var tableTitle = stackName.Substring(0, 2) + "...";
        BuildTableWithTitle(tableData, tableTitle, new string[] { "Front" });
    }

    private static List<List<object>> FormatTableData(FlashcardDto flashcard)
    {
        var originalText =  new List<object>() { flashcard.Front };
        return new List<List<object>>() { originalText };
    }

    private static void BuildTable(List<List<object>> tableData, params string[] columnNames)
    {
        ConsoleTableBuilder
            .From(tableData)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .WithColumn(columnNames)
            .ExportAndWriteLine();
        Console.WriteLine();
    }

    private static void BuildTableWithTitle(List<List<object>> tableData, string title, params string[] columnNames)
    {
        ConsoleTableBuilder
            .From(tableData)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .WithTitle(title)
            .WithColumn(columnNames)
            .ExportAndWriteLine();
        Console.WriteLine();
    }

    private static List<List<object>> FormatTableData(List<string> stacks)
    {
        return stacks.Select(s => new List<object>
            {
                s,
            }).ToList();
    }

    private static List<List<object>> FormatTableData(FlashcardStackDto stack)
    {
        return stack.Flashcards.Select(s => new List<object>
            {
                s.ViewId,
                s.Front,
                s.Back,
            }).ToList();
    }

    private static List<List<object>> FormatTableData(List<StudySessionDto> sessions)
    {
        return sessions.Select(s => new List<object>
            {
                s.StackName,
                s.Date,
                s.Score,
            }).ToList();
    }
}
