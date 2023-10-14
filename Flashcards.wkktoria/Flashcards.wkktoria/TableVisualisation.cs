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
}