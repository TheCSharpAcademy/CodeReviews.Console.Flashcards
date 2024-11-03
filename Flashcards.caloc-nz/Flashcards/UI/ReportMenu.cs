using Flashcards.Data;
using Flashcards.Models;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.UI;

public class ReportMenu
{
    public static void Show(AppDbContext dbContext)
    {
        var reportService = new ReportService(dbContext);
        var isRunning = true;

        while (isRunning)
        {
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a report option")
                    .AddChoices("Filter Sessions by Date", "Filter Sessions by Score", "Weekly Statistics",
                        "Cumulative Progress", "Back"));

            switch (selection)
            {
                case "Filter Sessions by Date":
                    var startDateInput = AnsiConsole.Prompt(
                        new TextPrompt<string>("Enter start date (YYYY-MM-DD) or leave empty for no start date:")
                            .AllowEmpty());
                    var startDate = ParseDate(startDateInput);

                    var endDateInput = AnsiConsole.Prompt(
                        new TextPrompt<string>("Enter end date (YYYY-MM-DD) or leave empty for no end date:")
                            .AllowEmpty());
                    var endDate = ParseDate(endDateInput);

                    var dateFilteredSessions = reportService.FilterSessionsByDate(startDate, endDate);
                    DisplaySessions(dateFilteredSessions);
                    break;

                case "Filter Sessions by Score":
                    var minScore = AnsiConsole.Ask<int>("Enter the minimum score to filter by:");
                    var scoreFilteredSessions = reportService.FilterSessionsByScore(minScore);
                    DisplaySessions(scoreFilteredSessions);
                    break;

                case "Weekly Statistics":
                    reportService.DisplayWeeklyStatistics();
                    break;

                case "Cumulative Progress":
                    reportService.DisplayCumulativeProgress();
                    break;

                case "Back":
                    isRunning = false;
                    break;
            }
        }
    }

    private static DateTime? ParseDate(string input)
    {
        return DateTime.TryParse(input, out var date) ? date.Date : null;
    }

    private static void DisplaySessions(List<StudySession> sessions)
    {
        if (sessions == null || sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No sessions found matching the criteria.[/]");
            return;
        }

        var table = new Table();
        table.AddColumn("Date");
        table.AddColumn("Score");
        table.AddColumn("Stack ID");

        foreach (var session in sessions)
            table.AddRow(session.Date.ToString("yyyy-MM-dd"), $"{session.Score}%", session.StackId.ToString());

        AnsiConsole.Write(table);
    }
}