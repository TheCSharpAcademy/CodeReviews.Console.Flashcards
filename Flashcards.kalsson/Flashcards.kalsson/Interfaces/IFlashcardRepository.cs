using Flashcards.kalsson.DTOs;

namespace Flashcards.kalsson.Interfaces;

public interface IFlashcardRepository
{
    Task<IEnumerable<FlashcardDTO>> GetFlashcardsByStackIdAsync(int stackId);
    Task<int> AddFlashcardAsync(FlashcardDTO flashcard);
    Task DeleteFlashcardAsync(int flashcardId);
}