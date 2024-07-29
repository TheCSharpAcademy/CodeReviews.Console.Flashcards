using Flashcards.Models;

namespace Flashcards.Repositories;
public class StudySessionRepository : BaseRepository<StudySession>, IStudySessionRepository {
    public StudySessionRepository(AppDbContext dbContext) : base(dbContext) {
    }
}
