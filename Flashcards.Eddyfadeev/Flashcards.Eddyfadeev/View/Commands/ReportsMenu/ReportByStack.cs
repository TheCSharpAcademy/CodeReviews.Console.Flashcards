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
/// Represents a command to retrieve a report by stack.
/// </summary>
internal sealed class ReportByStack : ICommand
{
    private readonly IStacksRepository _stacksRepository;
    private readonly IStudySessionsRepository _studySessionsRepository;
    private readonly IEditableEntryHandler<IStack> _stackEntryHandler;
    
    public ReportByStack(
        IStacksRepository stacksRepository,
        IStudySessionsRepository studySessionsRepository,
        IEditableEntryHandler<IStack> stackEntryHandler
        )
    {
        _stacksRepository = stacksRepository;
        _studySessionsRepository = studySessionsRepository;
        _stackEntryHandler = stackEntryHandler;
    }
    
    public void Execute()
    {
        var stack = StackChooserService.GetStackFromUser(_stacksRepository, _stackEntryHandler);
        var studySessions = _studySessionsRepository.GetByStackId(stack).ToList();
        
        if (studySessions.Count == 0)
        {
            AnsiConsole.MarkupLine(Messages.Messages.NoEntriesFoundMessage);
            GeneralHelperService.ShowContinueMessage();
            return;
        }
        
        var reportStrategy = new ByStackReportStrategy(studySessions);
        var reportGenerator = new ReportGenerator<IStudySession>(reportStrategy, ReportType.ReportByStack);
        
        var table = reportGenerator.GetReportToDisplay();
        AnsiConsole.Write(table);
        
        reportGenerator.SaveReportToPdf();
        
        GeneralHelperService.ShowContinueMessage();
    }
}