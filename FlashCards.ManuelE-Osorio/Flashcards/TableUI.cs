using ConsoleTableExt;

namespace Flashcards;

class TableUI
{
    public static string StacksTitle = "Stacks";
    public static string[] StacksHeader = ["Stack ID", "Stack Name"];
    public static string[] CardsHeader = ["Card ID", "Question", "Answer"];
    public static string StudySessionsTitle = "Study Sessions";
    public static string[] StudySessionsHeader = ["Study Session ID","Stack Name", "Date", "Score"];
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

    public static void PrintCardsTable(List<CardsDTO> cardsToUI, string? stackName)
    {
                List<List<object>> listToUI = [];;      
        foreach (CardsDTO cardsDTO in cardsToUI)
        {
            listToUI.Add([cardsDTO.CardID, cardsDTO.Question, cardsDTO.Answer]);
        }
        
        ConsoleTableBuilder
        .From(listToUI)
        .WithTitle(stackName)
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

    public static void PrintStudySessionsTable(List<StudySessionDTO> studySessionsToUI)
    {
        List<List<object>> listToUI = [];;      
        foreach (StudySessionDTO studySessionDTO in studySessionsToUI)
        {
            listToUI.Add([studySessionDTO.StudySessionID, studySessionDTO.StackName, studySessionDTO.Date, studySessionDTO.Score]);
        }
        
        ConsoleTableBuilder
        .From(listToUI)
        .WithTitle(StudySessionsTitle)
        .WithColumn(StudySessionsHeader)
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