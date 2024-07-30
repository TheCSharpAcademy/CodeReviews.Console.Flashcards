using Flashcards.Models;

namespace Flashcards.Repositories;
public class FlashcardRepository : BaseRepository<Flashcard>, IFlashcardRepository {
    public FlashcardRepository(AppDbContext dbContext) : base(dbContext) { }
}
