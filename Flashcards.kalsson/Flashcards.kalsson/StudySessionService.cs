using Flashcards.kalsson.Data;
using Flashcards.kalsson.Models;

namespace Flashcards.kalsson;

public class StudySessionService
{
    private readonly StudySessionRepository _studySessionRepository;

    public StudySessionService(StudySessionRepository studySessionRepository)
    {
        _studySessionRepository = studySessionRepository;
    }

    public IEnumerable<StudySession> GetAllStudySessions()
    {
        return _studySessionRepository.GetAllStudySessions();
    }

    public void AddStudySession(StudySession studySession)
    {
        _studySessionRepository.AddStudySession(studySession);
    }
}