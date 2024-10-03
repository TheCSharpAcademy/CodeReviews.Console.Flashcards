using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Repositories.Interfaces;

public interface IFlashcardRepository
{
    Task AddFlashcard(Flashcard Flashcard);
    Task DeleteFlashcard(int id);
    Task UpdateFlashcard(Flashcard Flashcard);
    Task<Flashcard> GetFlashcard(int id);
    Task<IEnumerable<Flashcard>> GetAllFlashcards();

}