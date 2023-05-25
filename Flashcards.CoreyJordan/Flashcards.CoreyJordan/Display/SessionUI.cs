using ConsoleTableExt;
using Flashcards.CoreyJordan.DTOs;

namespace Flashcards.CoreyJordan.Display;
internal class SessionUI
{
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
