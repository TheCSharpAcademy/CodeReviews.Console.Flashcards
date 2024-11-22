using CodingTracker.Helpers;
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker.Views;

public static class TableVisualisation
{
    public static void ShowCodingTable(List<CodingSession> codingSessions)
    {
        var table = new Table().AddColumns("ID", "Start Time", "End Time", "Duration (Hours)");
        if (!Utilities.CheckSessionsExist(codingSessions)) return;
        foreach (CodingSession session in codingSessions)
        {
            table.AddRow(
                session.Id.ToString(),
                session.StartTime,
                session.EndTime,
                session.Duration.ToString());
        }
        AnsiConsole.Write(table);
    }
    
    public static void ShowGoalsTable(List<Goal> goals)
    {
        var table = new Table().AddColumns("ID", "Start Date", "Date to Complete By", "Total Hours");
        if (!Utilities.CheckGoalsExist(goals)) return;
        foreach (Goal goal in goals)
        {
            table.AddRow(
                goal.Id.ToString(),
                goal.StartDate,
                goal.DateToComplete,
                goal.Hours.ToString()
            );
        }
        AnsiConsole.Write(table);
    }
}