using Flashcards.Models;

namespace Flashcards.Repositories;
public class FlashcardRepository : BaseRepository<Flashcard>, IFlashcardRepository {
    public FlashcardRepository(AppDbContext _dbContext) : base(_dbContext) { }
    public Task GetByStackName(string stack) {
        throw new NotImplementedException();
    }
}
