using FlashcardsLibrary.Models;
using Spectre.Console;

namespace FlashardsUI;
internal class TableVisualization
{
    internal static void ShowStacksTable(List<StackDTO> tableDataDTO)
    {
        var table = new Table();

        table.Title = new TableTitle("Stacks Table", "bold");

        table.AddColumn("Name");

        foreach (var stackDTO in tableDataDTO)
        {
            table.AddRow(stackDTO.Name!);
        }

        AnsiConsole.Write(table);
    }

    internal static void ShowFlashcardsTable(List<FlashcardDTO> tableDataDTO, string stackName)
    {
        var table = new Table();

        table.Title = new TableTitle($"Flashcards Table for {stackName} stack", "bold");

        table.AddColumns("Question", "Answer");

        foreach (var flashcardDTO in tableDataDTO)
        {
            table.AddRow(flashcardDTO.Question!, flashcardDTO.Answer!);
        }

        AnsiConsole.Write(table);
    }

    internal static void ShowStudySessionsTable(IEnumerable<StudySessionDTO> tableDataDTO)
    {
        var table = new Table();

        table.Title = new TableTitle("Study sessions table", "bold");

        table.AddColumns("Stack", "Date", "Score");

        foreach (var sessionDTO in tableDataDTO)
        {
            table.AddRow(sessionDTO.Name.ToString(), sessionDTO.Date.ToString("g"), sessionDTO.Score.ToString());
        }

        AnsiConsole.Write(table);
    }
}
