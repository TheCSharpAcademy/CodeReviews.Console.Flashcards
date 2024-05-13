using FlashcardsLibrary.Models;

namespace FlashcardsLibrary.Repositories;
public interface IStudySessionsRepository
{
    Task<IEnumerable<StudySession>> GetAllAsync();
    Task AddAsync(StudySession session);
}
