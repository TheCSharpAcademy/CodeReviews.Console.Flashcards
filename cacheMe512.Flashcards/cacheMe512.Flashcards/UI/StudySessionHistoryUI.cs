using Spectre.Console;
using cacheMe512.Flashcards.Controllers;

namespace cacheMe512.Flashcards.UI;

internal class StudySessionHistoryUI
{
    private static readonly StudySessionController _sessionController = new();

    public void Show()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[bold yellow]=== Study Session History ===[/]");

        var sessions = _sessionController.GetAllSessions()
            .OrderByDescending(s => s.Date)
            .ThenBy(s => s.StackName)
            .ThenByDescending(s => s.Score)
            .ToList();

        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("[red]No study sessions available.[/]");
            Console.ReadKey();
            return;
        }

        var table = new Table();
        table.AddColumn("[yellow]Date[/]");
        table.AddColumn("[yellow]Stack Name[/]");
        table.AddColumn("[yellow]Score[/]");
        table.AddColumn("[yellow]Total Questions[/]");

        foreach (var session in sessions)
        {
            table.AddRow(
                session.Date.ToString("yyyy-MM-dd HH:mm"),
                session.StackName,
                session.Score.ToString(),
                session.TotalQuestions.ToString()
            );
        }

        AnsiConsole.Write(table);

        AnsiConsole.MarkupLine("\nPress Any Key to Return to Main Menu.");
        Console.ReadKey();
    }
}
