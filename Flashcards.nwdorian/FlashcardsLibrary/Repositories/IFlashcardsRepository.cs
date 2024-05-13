using FlashcardsLibrary.Models;

namespace FlashcardsLibrary.Repositories;
public interface IFlashCardsRepository
{
    Task<IEnumerable<Flashcard>> GetAllAsync(Stack stack);
    Task AddAsync(Flashcard flashcard);
    Task UpdateAsync(Flashcard flashcard);
    Task DeleteAsync(Flashcard flashcard);
    Task<bool> FlashcardExistsAsync(string name);
}
