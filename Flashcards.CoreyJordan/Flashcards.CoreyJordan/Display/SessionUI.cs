using ConsoleTableExt;
using Flashcards.CoreyJordan.DTOs;

namespace Flashcards.CoreyJordan.Display;
internal class SessionUI
{
    private List<MenuModel> ReportMenu { get; } = new()
    {
        new MenuModel("1", "View Full Report"),
        new MenuModel("2", "View by Date"),
        new MenuModel("3", "View by Pack"),
        new MenuModel("X", "Exit Report Card")
    };

    internal void DisplayEndOfSession(SessionDTO session)
    {
        List<SessionDTO> sessions = new()
        {
            session
        };
        ConsoleTableBuilder
            .From(sessions)
            .WithFormat(ConsoleTableBuilderFormat.MarkDown)
            .ExportAndWriteLine(TableAligntment.Center); 
    }
}
