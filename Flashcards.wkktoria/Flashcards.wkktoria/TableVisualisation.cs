using ConsoleTableExt;
using Flashcards.wkktoria.Models.Dtos;

namespace Flashcards.wkktoria;

internal static class TableVisualisation
{
    internal static void ShowStacksTable(List<StackDto> stacks)
    {
        ConsoleTableBuilder
            .From(stacks)
            .WithTitle("Stacks")
            .WithColumn("Id", "Name")
            .ExportAndWriteLine();
    }

    internal static void ShowCardsTable(List<CardDto> cards)
    {
        ConsoleTableBuilder
            .From(cards)
            .WithTitle("Cards")
            .WithColumn("Id", "Front", "Back")
            .ExportAndWriteLine();
    }

    internal static void ShowSessionsTable(List<SessionDto> sessions)
    {
        ConsoleTableBuilder
            .From(sessions)
            .WithTitle("Sessions")
            .WithColumn("Date", "Score")
            .ExportAndWriteLine();
    }

    internal static void ShowSessionsInYear(List<ReportDataDto.ReportDataSessions> data, int year)
    {
        ConsoleTableBuilder
            .From(data)
            .WithTitle($"Sessions per month in {year}")
            .WithColumn("Month", "Sessions")
            .ExportAndWriteLine();
    }

    internal static void ShowAverageScoresInYear(List<ReportDataDto.ReportDataAverageScores> data, int year)
    {
        ConsoleTableBuilder
            .From(data)
            .WithTitle($"Average scores per month in {year}")
            .WithColumn("Month", "Name", "Score")
            .ExportAndWriteLine();
    }
}