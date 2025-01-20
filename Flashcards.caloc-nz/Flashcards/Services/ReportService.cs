using System.Globalization;
using Flashcards.Data;
using Flashcards.Helpers;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.Services;

public class ReportService
{
    private readonly AppDbContext _dbContext;

    public ReportService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void DisplayWeeklyStatistics()
    {
        var studySessions = _dbContext.StudySessions
            .AsEnumerable()
            .GroupBy(ss => new { ss.StackId, Week = GetWeekOfYear(ss.Date) })
            .Select(group => new
            {
                group.Key.StackId,
                group.Key.Week,
                SessionCount = group.Count(),
                AverageScore = group.Average(ss => ss.Score)
            })
            .OrderBy(stat => stat.StackId)
            .ThenBy(stat => stat.Week)
            .ToList();

        if (!studySessions.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No study sessions available for weekly statistics.[/]");
            return;
        }

        AnsiConsole.MarkupLine("[bold yellow]Weekly Session Statistics:[/]");
        foreach (var stat in studySessions)
            StatisticsHelper.DisplayStackStatistics(_dbContext, stat.StackId, $"Week {stat.Week}", stat.SessionCount,
                stat.AverageScore);
    }

    public void DisplayCumulativeProgress()
    {
        var cumulativeStats = _dbContext.StudySessions
            .GroupBy(ss => ss.StackId)
            .Select(group => new
            {
                StackId = group.Key,
                TotalSessions = group.Count(),
                AverageScore = group.Average(ss => ss.Score)
            })
            .OrderBy(stat => stat.StackId)
            .ToList();

        if (!cumulativeStats.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No study sessions available for cumulative statistics.[/]");
            return;
        }

        AnsiConsole.MarkupLine("[bold yellow]Cumulative Progress Statistics:[/]");
        foreach (var stat in cumulativeStats)
            StatisticsHelper.DisplayStackStatistics(_dbContext, stat.StackId, "Total", stat.TotalSessions,
                stat.AverageScore);
    }

    // New method to filter sessions by date range
    public List<StudySession> FilterSessionsByDate(DateTime? startDate, DateTime? endDate)
    {
        var query = _dbContext.StudySessions.AsQueryable();

        if (startDate.HasValue) query = query.Where(ss => ss.Date >= startDate.Value);

        if (endDate.HasValue) query = query.Where(ss => ss.Date <= endDate.Value);

        return query.ToList();
    }

    // New method to filter sessions by minimum score
    public List<StudySession> FilterSessionsByScore(int minScore)
    {
        return _dbContext.StudySessions
            .Where(ss => ss.Score >= minScore)
            .ToList();
    }

    private int GetWeekOfYear(DateTime date)
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        return cultureInfo.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay,
            DayOfWeek.Monday);
    }
}