using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Exceptions;
using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Interfaces.View.Commands;
using Flashcards.Eddyfadeev.Interfaces.View.Factory;
using Flashcards.Eddyfadeev.View.Commands.ReportsMenu;

namespace Flashcards.Eddyfadeev.View.Factory.EntriesInitializers;

internal class ReportsMenuEntriesInitializer : IMenuEntriesInitializer<ReportsMenuEntries>
{
    private readonly IStudySessionsRepository _studySessionsRepository;
    private readonly IStacksRepository _stacksRepository;
    private readonly IEditableEntryHandler<IStack> _stackEntryHandler;
    private readonly IEditableEntryHandler<IYear> _yearEntryHandler;
    
    public ReportsMenuEntriesInitializer(
        IStudySessionsRepository studySessionsRepository,
        IStacksRepository stacksRepository,
        IEditableEntryHandler<IStack> stackEntryHandler,
        IEditableEntryHandler<IYear> yearEntryHandler)
    {
        _studySessionsRepository = studySessionsRepository;
        _stacksRepository = stacksRepository;
        _stackEntryHandler = stackEntryHandler;
        _yearEntryHandler = yearEntryHandler;
    }

    public Dictionary<ReportsMenuEntries, Func<ICommand>> InitializeEntries(
        IMenuCommandFactory<ReportsMenuEntries> menuCommandFactory) =>
        new()
        {
            { ReportsMenuEntries.FullReport, () => new FullReport(_studySessionsRepository) },
            { ReportsMenuEntries.ReportByStack, () => new ReportByStack(
                _stacksRepository,
                _studySessionsRepository, 
                _stackEntryHandler
                ) 
            },
            { ReportsMenuEntries.AverageYearlyReport, () => new AverageYearlyReport(_studySessionsRepository, _yearEntryHandler) },
            { ReportsMenuEntries.ReturnToMainMenu, () => throw new ReturnToMainMenuException() }
        };
}