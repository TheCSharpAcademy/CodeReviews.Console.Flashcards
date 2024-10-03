using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Services.Interfaces;

public interface IFlashcardService
{
    Task<Result<string>> AddFlashcard(Flashcard flashcard);
    Task<Result<string>> DeleteFlashcard(int id);
    Task<Result<string>> UpdateFlashcard(Flashcard flashcard);
    Task<Result<Flashcard>> GetFlashcard(int id);
    Task<Result<IEnumerable<Flashcard>>> GetAllFlashcard();
}