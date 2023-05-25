using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.DTOs;
internal class SessionDTO
{
    public string Player { get; set; }
    public string Pack { get; set; }
    public int PackSize { get; set; }
    public DateTime Date { get; set; }
    public int Cycles { get; set; }

    public SessionDTO(SessionModel session)
    {
        Player = session.Player;
        Pack = session.Pack;
        PackSize = session.PackSize;
        Date = session.Date;
        Cycles = session.Cycles;
    }
}
