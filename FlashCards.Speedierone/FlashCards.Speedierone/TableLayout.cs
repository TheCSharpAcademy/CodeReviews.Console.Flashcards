using ConsoleTableExt;
using System.Data;

namespace FlashCards;

internal class TableLayout
{
    public static void DisplayTableStack(List<StudySessions> sessions)
    {
        var tableData = new List<List<Object>>();
        foreach (StudySessions studySessions in sessions)
        {
            tableData.Add(new List<Object>
            {
                studySessions.Subject
            });
        }
        ConsoleTableBuilder.From(tableData).WithColumn("Subject").ExportAndWriteLine();
    }
    public static void DisplayTableFlashCard(List<StudySessions> sessions)
    {
        var tableData = new List<List<Object>>();
        foreach (StudySessions studySessions in sessions)
        {
            tableData.Add(new List<Object>
            {
                studySessions.ID,
                studySessions.FlashSubject,
                studySessions.FrontCard,
                studySessions.BackCard
            });
        }
        ConsoleTableBuilder.From(tableData).WithColumn("ID", "Subject", "FrontOfCard", "BackOfCard").ExportAndWriteLine();
    }
    public static void DisplayGameHistory(List<StudySessions> sessions)
    {
        var tableData = new List<List<Object>>();
        foreach (StudySessions studySessions in sessions)
        {
            tableData.Add(new List<Object>
            {
                studySessions.Date.ToString("dd-MM-yyyy"),
                studySessions.Subject,
                studySessions.GameScore,
                studySessions.GameAmount
            });
        }
        ConsoleTableBuilder.From(tableData).WithColumn("Date", "Subject", "GameScore", "GameAmount").ExportAndWriteLine();
    }
    public static void DisplayScoreMonthly(DataTable sessions)
    {        
        ConsoleTableBuilder.From(sessions).ExportAndWriteLine();
        Console.WriteLine("\nPress any button to continue.");
        Console.ReadLine();
    }
    public static void DisplayAverageScore(DataTable score)
    {
        ConsoleTableBuilder.From(score).ExportAndWriteLine();
        Console.WriteLine("\nPress any button to continue.");
        Console.ReadLine();
    }
}
