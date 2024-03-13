using Flashcards.Dejmenek.Models;

namespace Flashcards.Dejmenek.DataAccess.Interfaces;

internal interface IFlashcardsRepository
{
    void AddFlashcard(int stackId, string front, string back);
    void DeleteFlashcard(int flashcardId);
    void UpdateFlashcard(int flashcardId, string front, string back);
    IEnumerable<Flashcard> GetAllFlashcards();
}
