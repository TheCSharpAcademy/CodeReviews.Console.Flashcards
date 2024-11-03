using Flashcards.Data;
using Flashcards.Helpers;
using Flashcards.Models;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.UI;

public class StudyMenu
{
    public static void Show(AppDbContext dbContext)
    {
        var studySessionService = new StudySessionService(dbContext);
        var isRunning = true;

        while (isRunning)
        {
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a study option")
                    .AddChoices("Start Study Session", "View All Sessions", "Filter Sessions by Date",
                        "Filter Sessions by Score", "Back"));

            switch (selection)
            {
                case "Start Study Session":
                    var stackId = AnsiConsole.Ask<int>("Enter the stack ID to study:");
                    if (!ValidationHelper.ValidateId(stackId, "stack")) break;

                    studySessionService.StartStudySession(stackId);
                    break;

                case "View All Sessions":
                    studySessionService.ViewStudySessions();
                    break;

                case "Filter Sessions by Date":
                    var startDateInput = AnsiConsole.Prompt(
                        new TextPrompt<string>("Enter start date (leave empty for no start date):").AllowEmpty());
                    var startDate = ParseDate(startDateInput);

                    var endDateInput = AnsiConsole.Prompt(
                        new TextPrompt<string>("Enter end date (leave empty for no end date):").AllowEmpty());
                    var endDate = ParseDate(endDateInput);

                    var dateFilteredSessions = studySessionService.FilterSessionsByDate(startDate, endDate);
                    DisplaySessions(dateFilteredSessions);
                    break;

                case "Filter Sessions by Score":
                    var minScore = AnsiConsole.Ask<int>("Enter the minimum score to filter by:");
                    var scoreFilteredSessions = studySessionService.FilterSessionsByScore(minScore);
                    DisplaySessions(scoreFilteredSessions);
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