using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Interfaces.View.Commands;
using Flashcards.Eddyfadeev.Report;
using Flashcards.Eddyfadeev.Report.Strategies;
using Flashcards.Eddyfadeev.Services;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.View.Commands.ReportsMenu;

/// <summary>
/// Represents a command to retrieve an average yearly report.
/// </summary>
internal sealed class AverageYearlyReport : ICommand
{
    private readonly IStudySessionsRepository _studySessionsRepository;
    private readonly IEditableEntryHandler<IYear> _yearEntryHandler;
    public AverageYearlyReport(
        IStudySessionsRepository studySessionsRepository, 
        IEditableEntryHandler<IYear> yearEntryHandler)
    {
        _studySessionsRepository = studySessionsRepository;
        _yearEntryHandler = yearEntryHandler;
    }

    public void Execute()
    {
        var selectedYear = StudySessionsHelperService.GetYearFromUser(_studySessionsRepository, _yearEntryHandler);
        var studySessions = _studySessionsRepository.GetAverageYearly(selectedYear).ToList();
        
        if (studySessions.Count == 0)
        {
            AnsiConsole.MarkupLine(Messages.Messages.NoEntriesFoundMessage);
            GeneralHelperService.ShowContinueMessage();
            return;
        }
        
        var reportStrategy = new AverageYearlyReportStrategy(studySessions, selectedYear);
        var reportGenerator = new ReportGenerator<IStackMonthlySessions>(reportStrategy, ReportType.AverageYearlyReport);
        
        var table = reportGenerator.GetReportToDisplay();
        AnsiConsole.Write(table);
        
        reportGenerator.SaveReportToPdf();
        
        GeneralHelperService.ShowContinueMessage();
    }
}