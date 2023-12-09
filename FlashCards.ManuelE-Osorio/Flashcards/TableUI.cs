using ConsoleTableExt;

namespace Flashcards;

class TableUI
{
    public static string StacksTitle = "Stacks";
    public static string[] StacksHeader = ["Stack ID", "Stack Name"];
    public static void PrintTable(List<List<object>> data)
    {  
        Helpers.ClearConsole();
        
        ConsoleTableBuilder
        .From(data)
        .WithTitle(StacksTitle)
        .WithColumn(StacksHeader)
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
        .ExportAndWriteLine(TableAligntment.Center);
    }
}