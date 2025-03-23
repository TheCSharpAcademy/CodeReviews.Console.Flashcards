using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsAssist.DreamFXX.Models;
using FlashcardsAssist.DreamFXX.Data;
using Spectre.Console;

namespace FlashcardsAssist.DreamFXX.Services;
public class SessionsService
{
    private readonly DatabaseService _dbService;

    public SessionsService(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    public async Task RecordStudySessionAsync(int stackId, int score)
    {
        await _dbService.RecordStudySessionAsync(stackId, DateTime.Now, score);
    }

    public async Task ViewStudyHistoryAsync()
    {
        var sessions = await _dbService.GetAllStudySessionsAsync();

        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No study sessions found.[/]");
            return;
        }

        var table = new Table()
            .Title("[yellow]Study History[/]")
            .AddColumn(new TableColumn("ID").Centered())
            .AddColumn(new TableColumn("Stack").LeftAligned())
            .AddColumn(new TableColumn("Date").LeftAligned())
            .AddColumn(new TableColumn("Score").Centered());

        foreach (var session in sessions)
        {
            table.AddRow(
                session.Id.ToString(),
                session.StackName,
                session.StudyDate.ToString("g"),
                session.Score.ToString()
            );
        }

        AnsiConsole.Write(table);
    }
}
