using Flashcards.kalsson.Data;
using Flashcards.kalsson.Models;

namespace Flashcards.kalsson.Services;

public class FlashcardService
{
    private readonly FlashcardRepository _flashcardRepository;

    public FlashcardService(FlashcardRepository flashcardRepository)
    {
        _flashcardRepository = flashcardRepository;
    }

    public IEnumerable<Flashcard> GetAllFlashcards(int stackId)
    {
        return _flashcardRepository.GetAllFlashcards(stackId);
    }

    public Flashcard GetFlashcardById(int id)
    {
        return _flashcardRepository.GetFlashcardById(id);
    }

    public void AddFlashcard(Flashcard flashcard)
    {
        _flashcardRepository.AddFlashcard(flashcard);
    }

    public void DeleteFlashcard(int id)
    {
        _flashcardRepository.DeleteFlashcard(id);
    }
}