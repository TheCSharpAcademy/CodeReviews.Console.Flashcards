using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Repositories.Interfaces;

public interface IStudySessionRepository
{
    Task AddStudySession(StudySession studySession);
    Task DeleteStudySession(int id);
    Task UpdateStudySession(StudySession studySession);
    Task<StudySession> GetStudySessionById(int id);
    Task<IEnumerable<StudySession>> GetAllStudySessions();
    Task<IEnumerable<StudySession>> GetStudySessionsByStackName(string name);
}