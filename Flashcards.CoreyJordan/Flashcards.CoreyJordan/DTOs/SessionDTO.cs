using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.DTOs;
internal class SessionDto
{
    public int Number { get; private set; }
    public string Player { get; }
    public string Pack { get; }
    public int PackSize { get; }
    public DateTime Date { get; }
    public double Score { get; }

    public SessionDto(SessionModel session, int number = 1)
    {
        Player = session.Player;
        Pack = session.Pack;
        PackSize = session.PackSize;
        Date = session.Date;
        Score = session.Score;
        Number = number;
    }

    public static List<SessionDto> GetSessionDtoList(List<SessionModel> sessions)
    {
        List<SessionDto> sessionData = new();
        for (int i = 0; i < sessions.Count; i++)
        {
            sessionData.Add(new SessionDto(sessions[i], i + 1));
        }
        return sessionData;
    }
}
