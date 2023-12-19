using ConsoleTableExt;

namespace Flashcards;

class TableUI
{
    public static string StacksTitle = "Stacks";
    public static string[] StacksHeader = ["Stack ID", "Stack Name"];
    public static string[] CardsHeader = ["Card ID", "Question", "Answer"];
    public static void PrintStacksTable(List<StacksDTO> currentStacksToUI)
    {  
        List<List<object>> listToUI = [];;      
        foreach (StacksDTO stacksDTO in currentStacksToUI)
        {
            listToUI.Add([stacksDTO.StackID, stacksDTO.StackName]);
        }
        
        ConsoleTableBuilder
        .From(listToUI)
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
    
    Console.WriteLine();
    }

    public static void PrintCardsTable(List<CardsDTO> cardsToUI, Stacks? currentStack)
    {
                List<List<object>> listToUI = [];;      
        foreach (CardsDTO cardsDTO in cardsToUI)
        {
            listToUI.Add([cardsDTO.CardID, cardsDTO.Question, cardsDTO.Answer]);
        }
        
        ConsoleTableBuilder
        .From(listToUI)
        .WithTitle(currentStack?.StackName)
        .WithColumn(CardsHeader)
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
    
    Console.WriteLine();
    }
}