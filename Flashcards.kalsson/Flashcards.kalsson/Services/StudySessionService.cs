using Flashcards.kalsson.Data;
using Flashcards.kalsson.Models;

namespace Flashcards.kalsson.Services;

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

    public StudySession GetStudySessionById(int id)
    {
        return _studySessionRepository.GetStudySessionById(id);
    }

    public void AddStudySession(StudySession studySession)
    {
        _studySessionRepository.AddStudySession(studySession);
    }
}