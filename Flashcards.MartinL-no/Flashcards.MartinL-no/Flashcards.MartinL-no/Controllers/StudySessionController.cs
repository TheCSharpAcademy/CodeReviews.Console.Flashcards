using Flashcards.MartinL_no.DAL;
using Flashcards.MartinL_no.Models;

namespace Flashcards.MartinL_no.Controllers;

internal class StudySessionController
{
    private StudySessionRepository _sessionRepo;

    public StudySessionController(StudySessionRepository sessionRepo)
    {
        _sessionRepo = sessionRepo;
    }

    public List<StudySession> GetSessions()
    {
        return _sessionRepo.GetSessions();
    }

    public List<StudySession> GetSessionByName(string name)
    {
        return _sessionRepo.GetSessionByName(name);
    }

    public bool AddStudySession(DateTime date, int score, int stackId)
    {
        if (score < 0) return false;

        var session = new StudySession()
        {
            Date = DateOnly.FromDateTime(date),
            Score = score,
            StackId = stackId
        };
        return _sessionRepo.InsertStudySession(session);
    }
}