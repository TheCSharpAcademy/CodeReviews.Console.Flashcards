using Flashcards.kalsson.Models;
using Flashcards.kalsson.Services;
using Spectre.Console;

namespace Flashcards.kalsson.UI;

public class StudySessionUI
{
    private readonly StudySessionService _studySessionService;
    private readonly StackService _stackService;

    public StudySessionUI(StudySessionService studySessionService, StackService stackService)
    {
        _studySessionService = studySessionService;
        _stackService = stackService;
    }

    public void ShowAllStudySessions()
    {
        var studySessions = _studySessionService.GetAllStudySessions();
        var table = new Table();

        table.AddColumn("ID");
        table.AddColumn("Stack Name");
        table.AddColumn("Study Date");
        table.AddColumn("Score");

        foreach (var session in studySessions)
        {
            var stack = _stackService.GetStackById(session.StackId);
            table.AddRow(session.Id.ToString(), stack?.Name ?? "Unknown", session.StudyDate.ToString(), session.Score.ToString());
        }

        AnsiConsole.Write(table);
    }

    public void AddStudySession()
    {
        var stackName = AnsiConsole.Ask<string>("Enter stack name:");
        var stack = _stackService.GetAllStacks().FirstOrDefault(s => s.Name == stackName);
        if (stack == null)
        {
            AnsiConsole.MarkupLine("[red]Stack not found.[/]");
            return;
        }

        var score = AnsiConsole.Ask<int>("Enter score:");
        var studySession = new StudySession
        {
            StackId = stack.Id,
            StudyDate = DateTime.Now,
            Score = score
        };

        _studySessionService.AddStudySession(studySession);
    }
}