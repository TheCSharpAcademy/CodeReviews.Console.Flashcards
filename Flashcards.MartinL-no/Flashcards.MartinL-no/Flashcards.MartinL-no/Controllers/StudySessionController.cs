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

    public List<StudySessionDTO> GetSessions()
    {
        var sessions = _sessionRepo.GetSessions();
        if (sessions.Count() == 0) return new List<StudySessionDTO>();

        return sessions.Select(s => StudySessionToDTO(s))
            .OrderBy(s => s.StackName)
            .ThenBy(s => s.Date).ToList();
    }

    public List<StudySessionDTO> GetSessionsByName(string name)
    {
        var sessions = _sessionRepo.GetSessionsByStackName(name);
        if (sessions == null) return new List<StudySessionDTO>();

        return sessions.Select(s => StudySessionToDTO(s))
            .OrderBy(s => s.StackName)
            .ThenBy(s => s.Date).ToList();
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

    private StudySessionDTO StudySessionToDTO(StudySession session)
    {
        return new StudySessionDTO() { Date = session.Date, Score = session.Score, StackName = session.StackName };
    }
}