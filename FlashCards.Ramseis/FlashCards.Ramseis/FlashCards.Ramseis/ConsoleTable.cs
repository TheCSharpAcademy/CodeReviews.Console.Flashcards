using ConsoleTableExt;

namespace FlashCards.Ramseis
{
    internal class ConsoleTable
    {
        public static void PrintTable(List<List<object>> data)
        {
            Console.Clear();
            ConsoleTableBuilder
                .From(data)
                .WithTitle("Study Session Scores")
                .WithColumn("Stack Name", "Score", "Questions", "Date")
                .WithTextAlignment(new Dictionary<int, TextAligntment>
                {
                    {0, TextAligntment.Left },
                    {1, TextAligntment.Center },
                    {2, TextAligntment.Center },
                    {3, TextAligntment.Right },
                })
                .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
                .WithCharMapDefinition(
                    CharMapDefinition.FramePipDefinition,
                    new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '═' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╤' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, ' ' },
                    })
                .ExportAndWriteLine();
        }
    }
}
