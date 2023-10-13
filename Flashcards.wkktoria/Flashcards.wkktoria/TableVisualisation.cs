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
            .WithColumn("No.", "Name")
            .ExportAndWriteLine();
    }

    internal static void ShowCardsTable(List<CardDto> cards)
    {
        ConsoleTableBuilder
            .From(cards)
            .WithTitle("Cards")
            .WithColumn("Front", "Back")
            .ExportAndWriteLine();
    }
}