using ConsoleTableExt;

namespace Flashcards.StevieTV.UI;

internal class TableVisualisation
{
    internal static void ShowTable<T>(List<T> tableData) where T : class
    {
        ConsoleTableBuilder
            .From(tableData)
            .WithCharMapDefinition(
                CharMapDefinition.FramePipDefinition,
                new Dictionary<HeaderCharMapPositions, char> {
                    {HeaderCharMapPositions.TopLeft, '╒' },
                    {HeaderCharMapPositions.TopCenter, '╤' },
                    {HeaderCharMapPositions.TopRight, '╕' },
                    {HeaderCharMapPositions.BottomLeft, '╞' },
                    {HeaderCharMapPositions.BottomCenter, '╪' },
                    {HeaderCharMapPositions.BottomRight, '╡' },
                    {HeaderCharMapPositions.BorderTop, '═' },
                    {HeaderCharMapPositions.BorderRight, '│' },
                    {HeaderCharMapPositions.BorderBottom, '═' },
                    {HeaderCharMapPositions.BorderLeft, '│' },
                    {HeaderCharMapPositions.Divider, '│' },
                })
            .ExportAndWriteLine();
        Console.WriteLine("\n");
    }
}