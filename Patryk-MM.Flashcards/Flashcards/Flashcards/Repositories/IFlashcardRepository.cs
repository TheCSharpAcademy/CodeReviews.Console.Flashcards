using Flashcards.Models;

namespace Flashcards.Repositories;
public interface IFlashcardRepository : IBaseRepository<Flashcard> {
    Task GetByStackName(string stack);
}

