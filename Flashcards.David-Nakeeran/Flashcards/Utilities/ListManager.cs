using Spectre.Console;
using Flashcards.Models;

namespace Flashcards.Utilities;

class ListManager
{
    internal List<FlashcardsDTO> flashcardsDTOs = new List<FlashcardsDTO>();
    internal List<StackDTO> stacksDTO = new List<StackDTO>();
    internal List<StudySessionDTO> studySessionsDTO = new List<StudySessionDTO>();
    internal List<StudySessionSummary> studySessionsSummaryDTO = new List<StudySessionSummary>();

    internal bool CheckForExistingStackRecord(string? name)
    {
        if (stacksDTO.Any(record => record.Name == name))
        {
            return true;
        }
        return false;
    }

    internal void PrintRecords()
    {
        foreach (var stack in stacksDTO)
        {
            AnsiConsole.MarkupLine("----------------------------------------------------------");
            AnsiConsole.MarkupLine($"ID: {stack.DisplayId} | Stack name: {stack.Name}");
            AnsiConsole.MarkupLine("----------------------------------------------------------");
            AnsiConsole.MarkupLine("");
        }
    }

    internal void PrintFlashcardRecords()
    {
        foreach (var record in flashcardsDTOs)
        {
            AnsiConsole.MarkupLine("----------------------------------------------------------");
            AnsiConsole.MarkupLine($"ID: {record.DisplayId} | Stack name: {record.Front} | Stack name: {record.Back} |  Stack name: {record.StackName} ");
            AnsiConsole.MarkupLine("----------------------------------------------------------");
            AnsiConsole.MarkupLine("");
        }
    }

    internal void PrintStudySessionRecords()
    {
        foreach (var record in studySessionsDTO)
        {
            AnsiConsole.MarkupLine("----------------------------------------------------------");
            AnsiConsole.MarkupLine($"Date: {record.Date} | Score: {record.Score} | Stack name: {record.StackName}");
            AnsiConsole.MarkupLine("----------------------------------------------------------");
            AnsiConsole.MarkupLine("");
        }
    }

    internal void PrintStudySessionSummary(List<StudySessionSummary> summaryList, int year)
    {
        var table = new Table();

        table.Title($"Study Session Summary for year: {year}");

        table.AddColumn("Stack name");
        table.AddColumn("January");
        table.AddColumn("February");
        table.AddColumn("March");
        table.AddColumn("April");
        table.AddColumn("May");
        table.AddColumn("June");
        table.AddColumn("July");
        table.AddColumn("August");
        table.AddColumn("September");
        table.AddColumn("October");
        table.AddColumn("November");
        table.AddColumn("December");

        foreach (var record in summaryList.Where(record => record != null))
        {
            table.AddRow(
            record.StackName?.ToString() ?? "N/A",
            record.January.ToString(),
            record.February.ToString(),
            record.March.ToString(),
            record.April.ToString(),
            record.May.ToString(),
            record.June.ToString(),
            record.July.ToString(),
            record.August.ToString(),
            record.September.ToString(),
            record.October.ToString(),
            record.November.ToString(),
            record.December.ToString());
        }

        AnsiConsole.Write(table);
    }

    internal bool IsListEmpty<T>(List<T> list) => list.Any();
}