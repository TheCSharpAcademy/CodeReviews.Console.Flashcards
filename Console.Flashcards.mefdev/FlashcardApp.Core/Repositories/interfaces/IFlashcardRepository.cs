using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Repositories.Interfaces;

public interface IFlashcardRepository
{
    Task AddFlashcard(Flashcard Flashcard);
    Task DeleteFlashcard(int id);
    Task DeleteFlashcardByQuestion(string question);
    Task UpdateFlashcard(Flashcard Flashcard);
    Task<Flashcard> GetFlashcard(int id);
    Task<Flashcard> GetFlashcardByQuestion(string question);
    Task<IEnumerable<Flashcard>> GetAllFlashcards();
    Task<IEnumerable<Flashcard>> GetFlashcardsByStackname(string name);
}
