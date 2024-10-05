using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Services.Interfaces;

public interface IFlashcardService
{
    Task<Result<string>> AddFlashcard(Flashcard flashcard);
    Task<Result<string>> DeleteFlashcard(int id);
    Task<Result<string>> DeleteFlashcardByQuestion(string question);
    Task<Result<string>> UpdateFlashcard(Flashcard flashcard);
    Task<Result<Flashcard>> GetFlashcard(int id);
    Task<Result<Flashcard>> GetFlashcardByQuestion(string question);
    Task<Result<IEnumerable<Flashcard>>> GetAllFlashcards();
    Task<Result<IEnumerable<Flashcard>>> GetFlashcardsByStackname(string name);
}