using Flashcards.Data;
using Flashcards.Helpers;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.Services;

public class StudySessionService
{
    private readonly AppDbContext _dbContext;

    public StudySessionService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void StartStudySession(int stackId)
    {
        if (!ValidationHelper.ValidateId(stackId, "stack")) return;

        var stack = _dbContext.Stacks.Find(stackId);
        if (stack == null)
        {
            AnsiConsole.MarkupLine("[red]Error: Stack could not be found.[/]");
            return;
        }

        var flashcards = _dbContext.Flashcards.Where(f => f.StackId == stackId).ToList();
        if (!flashcards.Any())
        {
            AnsiConsole.MarkupLine("[red]Error: No flashcards could be found in this stack.[/]");
            return;
        }

        var correctAnswers = 0;
        foreach (var flashcard in flashcards)
        {
            AnsiConsole.MarkupLine($"[yellow]Question:[/] {flashcard.Front}");
            var userAnswer = AnsiConsole.Ask<string>("Enter your answer:");

            if (string.Equals(userAnswer.Trim(), flashcard.Back.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                correctAnswers++;
                AnsiConsole.MarkupLine("[green]Correct![/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Incorrect.[/] The correct answer was: {flashcard.Back}");
            }
        }

        var score = (int)((double)correctAnswers / flashcards.Count * 100);

        var studySession = new StudySession
        {
            Date = DateTime.Now,
            Score = score,
            StackId = stackId
        };

        _dbContext.StudySessions.Add(studySession);
        _dbContext.SaveChanges();

        AnsiConsole.MarkupLine($"[green]Study session completed! Your score: {score}%[/]");
    }

    public void ViewStudySessions()
    {
        var sessions = _dbContext.StudySessions
            .OrderByDescending(ss => ss.Date)
            .ToList();

        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No study sessions found.[/]");
            return;
        }

        AnsiConsole.MarkupLine("[bold yellow]Past Study Sessions:[/]");
        foreach (var session in sessions)
        {
            var stack = _dbContext.Stacks.Find(session.StackId);
            var stackName = stack?.Name ?? "Unknown";

            AnsiConsole.MarkupLine($"[yellow]Date:[/] {session.Date:yyyy-MM-dd}  " +
                                   $"[yellow]Score:[/] {session.Score}%  " +
                                   $"[yellow]Stack:[/] {stackName}");
        }
    }

    public List<StudySession> FilterSessionsByDate(DateTime? startDate, DateTime? endDate)
    {
        var query = _dbContext.StudySessions.AsQueryable();

        if (startDate.HasValue) query = query.Where(ss => ss.Date >= startDate.Value);
        if (endDate.HasValue) query = query.Where(ss => ss.Date <= endDate.Value);

        return query.OrderByDescending(ss => ss.Date).ToList();
    }

    public List<StudySession> FilterSessionsByScore(int minScore)
    {
        return _dbContext.StudySessions
            .Where(ss => ss.Score >= minScore)
            .OrderByDescending(ss => ss.Score)
            .ToList();
    }
}