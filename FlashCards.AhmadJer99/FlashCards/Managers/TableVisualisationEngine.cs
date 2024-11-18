using ConsoleTableExt;

namespace FlashCards.Managers;
public class TableVisualisationEngine<T> where T : class //  enforces that T must be a reference type
{
    public static void ViewAsTable(List<T> listOfClassObjects, TableAligntment tableAligntment, List<string> columnNames,string tableTitle = "")
    {

        ConsoleTableBuilder.From(listOfClassObjects)
            .WithColumn(columnNames)
            .WithTitle(tableTitle)
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
                   .ExportAndWriteLine(tableAligntment);
    }

    public static void ViewSingleColumn(string value, string columnName, TableAligntment tableAligntment)
    {
        // Wrap the string in a list of objects
        var dataForTable = new List<dynamic> {  value };

        ConsoleTableBuilder.From(dataForTable)
            .WithColumn(new List<string> { columnName })
            .WithCharMapDefinition(
                CharMapDefinition.FramePipDefinition,
                new Dictionary<HeaderCharMapPositions, char>
                {
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
            .ExportAndWriteLine(tableAligntment);
    }
}
