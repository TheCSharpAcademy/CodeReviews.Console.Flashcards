using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.DTOs;
internal class SessionDTO
{
    public int Number { get; private set; }
    public string Player { get; }
    public string Pack { get; }
    public int PackSize { get; }
    public DateTime Date { get; }
    public double Score { get; }

    public SessionDTO(SessionModel session, int number = 0)
    {
        Player = session.Player;
        Pack = session.Pack;
        PackSize = session.PackSize;
        Date = session.Date;
        Score = session.Score;
        Number = number;
    }

    public static List<SessionDTO> GetCardsDTO(List<SessionModel> sessions)
    {
        List<SessionDTO> sessionData = new();
        for (int i = 0; i < sessions.Count; i++)
        {
            sessionData.Add(new SessionDTO(sessions[i], i + 1));
        }
        return sessionData;
    }
}
