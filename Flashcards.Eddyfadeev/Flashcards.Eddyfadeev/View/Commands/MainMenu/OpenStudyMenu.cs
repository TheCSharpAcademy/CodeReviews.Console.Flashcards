using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.View.Commands;

namespace Flashcards.Eddyfadeev.View.Commands.MainMenu;

/// <summary>
/// Represents a command to open the study menu.
/// </summary>
internal sealed class OpenStudyMenu : ICommand
{
    private readonly IMenuHandler<StudyMenuEntries> _studyMenuHandler;

    public OpenStudyMenu(IMenuHandler<StudyMenuEntries> studyMenuHandler)
    {
        _studyMenuHandler = studyMenuHandler;
    }
    
    public void Execute()
    {
        _studyMenuHandler.HandleMenu();
    }
}