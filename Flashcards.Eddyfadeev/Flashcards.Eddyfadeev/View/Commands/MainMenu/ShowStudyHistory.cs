using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.View.Commands;

namespace Flashcards.Eddyfadeev.View.Commands.MainMenu;

/// <summary>
/// Represents a command that displays the study history to the user and provides options to save it as a PDF.
/// </summary>
internal class ShowStudyHistory : ICommand
{
    private readonly IMenuHandler<ReportsMenuEntries> _reportsMenuHandler;
    
    public ShowStudyHistory(IMenuHandler<ReportsMenuEntries> reportsMenuHandler)
    {
        _reportsMenuHandler = reportsMenuHandler;
    }
    
    public void Execute()
    {
        _reportsMenuHandler.HandleMenu();
    }
}