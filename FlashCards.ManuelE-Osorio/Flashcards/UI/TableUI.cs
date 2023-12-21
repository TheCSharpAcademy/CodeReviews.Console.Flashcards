using ConsoleTableExt;

namespace Flashcards;

class TableUI
{
    public static string StacksTitle = "Stacks";
    public static string[] StacksHeader = ["Stack ID", "Stack Name"];
    public static string[] CardsHeader = ["Card ID", "Question", "Answer"];
    public static string StudySessionsTitle = "Study Sessions";
    public static string[] StudySessionsHeader = ["Study Session ID","Stack Name", "Date", "Score"];
    public static string ReportTitle = "Study Sessions Report";
    public static string[] ReportHeader = ["Year", "Month", "Stack Name", "Total Study Sessions", "Average Score"];
    public static void PrintStacksTable(List<StacksDTO> currentStacksToUI)
    {  
        List<List<object>> listToUI = [];;      
        foreach (StacksDTO stacksDTO in currentStacksToUI)
        {
            listToUI.Add([stacksDTO.StackID, stacksDTO.StackName]);
        }
        PrintTable(listToUI,StacksTitle,StacksHeader);       
    }

    public static void PrintCardsTable(List<CardsDTO> cardsToUI, string? stackName)
    {
        List<List<object>> listToUI = [];;      
        foreach (CardsDTO cardsDTO in cardsToUI)
        {
            listToUI.Add([cardsDTO.CardID, cardsDTO.Question, cardsDTO.Answer]);
        }
        PrintTable(listToUI, stackName,CardsHeader);
    }

    public static void PrintStudySessionsTable(List<StudySessionDTO> studySessionsToUI)
    {
        List<List<object>> listToUI = [];;      
        foreach (StudySessionDTO studySessionDTO in studySessionsToUI)
        {
            listToUI.Add([studySessionDTO.StudySessionID, studySessionDTO.StackName, studySessionDTO.Date, studySessionDTO.Score]);
        }
        PrintTable(listToUI, StudySessionsTitle, StudySessionsHeader);
    }

    public static void PrintCardQuestion(CardsDTO currentCard)
    {
        List<List<object>> listToUI = [];
        string[] columns = ["Question"];
        listToUI.Add([currentCard.Question]);
        PrintTable(listToUI,"Card", columns);
    }

    public static void PrintCardAnswer(CardsDTO currentCard)
    {
        List<List<object>> listToUI = [];
        string[] columns = ["Question", "Answer"];
        listToUI.Add([currentCard.Question, currentCard.Answer]);
        PrintTable(listToUI,"Card", columns);        
    }

    public static void PrintTable(List<List<object>> tableToUI,string? title, string[] columns)
    {
        ConsoleTableBuilder
        .From(tableToUI)
        .WithTitle(title)
        .WithColumn(columns)
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