using Flashcards.kalsson.Models;

namespace Flashcards.kalsson.Interfaces;

public interface IStudySessionRepository
{
    Task<int> AddStudySessionAsync(StudySession session);
    Task<IEnumerable<StudySession>> GetStudySessionsByStackIdAsync(int stackId);
    Task UpdateStudySessionAsync(StudySession session);
}