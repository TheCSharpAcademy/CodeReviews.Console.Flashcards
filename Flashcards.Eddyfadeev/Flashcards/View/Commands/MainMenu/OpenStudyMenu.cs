using Flashcards.Enums;
using Flashcards.Interfaces.Handlers;
using Flashcards.Interfaces.View.Commands;

namespace Flashcards.View.Commands.MainMenu;

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