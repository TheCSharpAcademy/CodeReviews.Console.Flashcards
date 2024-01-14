using Flashcards.Data;
using Flashcards.Models;

namespace Flashcards.Controllers;

public class FlashcardsController
{
    private readonly DataAccess _dataAccess;

    public FlashcardsController(DataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<List<Flashcard>> GetAllFlashcardsAsync()
    {
        return await _dataAccess.GetAllFlashcardsAsync();
    }

    public async Task<List<Flashcard>> GetFlashcardsByStackNameAsync(string stackName)
    {
        return await _dataAccess.GetFlashcardsByStackNameAsync(stackName);
    }

    public async Task<Flashcard> GetFlashcardByIdAsync(int id)
    {
        var flashcard = await _dataAccess.GetFlashcardByIdAsync(id);
        if (flashcard == null) return null;

        return flashcard;
    }

    public async Task<Flashcard> AddFlashcardAsync(int stackId, Flashcard newFlashcard)
    {
        var flashcard = await _dataAccess.AddFlashcardAsync(stackId, newFlashcard);
        if (flashcard == null) return null;

        return flashcard;
    }

    public async Task<Flashcard> UpdateFlashcardById(int flashcardId, Flashcard updateFlashcard)
    {
        var flashcard = await _dataAccess.UpdateFlashcardAsync(flashcardId, updateFlashcard);
        if (flashcard == null) return null;

        return flashcard;
    }

    public async Task<Flashcard> DeleteFlashcardById(int stackId, int flashcardId)
    {
        var flashcard = await _dataAccess.DeleteFlashcardAsync(stackId, flashcardId);
        if (flashcard == null) return null;

        return flashcard;
    }
}