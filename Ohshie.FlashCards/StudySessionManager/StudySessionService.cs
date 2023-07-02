using Ohshie.FlashCards.DataAccess;

namespace Ohshie.FlashCards.StudySessionManager;

public class StudySessionService
{
    private StudySessionRepository _sessionRepository = new();
    
    public void CreateSession(int correctAnswers, string deckName)
    {
        var session = new StudySession
        {
            Date = DateTime.Today
                .ToLocalTime()
                .ToString("dd.MM.yyyy"),
            DeckName = deckName,
            FlashcardsSolved = correctAnswers
        };
        
        _sessionRepository.CreateSession(session);
    }

    public List<StudySession> SessionsToDisplay()
    {
        var allSessions =_sessionRepository.FetchSessions();

        allSessions= allSessions.OrderBy(session => session.DeckName).ToList();

        return allSessions;
    }
}