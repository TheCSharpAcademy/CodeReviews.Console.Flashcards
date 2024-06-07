
using Flashcards.UndercoverDev.Models;

namespace Flashcards.UndercoverDev.Repository
{
    public interface IFlashcardRepository
    {
        List<Flashcard> GetFlashcards();
        void Post(FlashcardDTO newFlashcard);
    }
}