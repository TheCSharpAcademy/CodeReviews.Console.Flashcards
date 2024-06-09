
using Flashcards.UndercoverDev.Models;

namespace Flashcards.UndercoverDev.Repository
{
    public interface IFlashcardRepository
    {
        void Delete(Flashcard flashcard);
        List<Flashcard> GetFlashcards();
        List<Flashcard> GetFlashcardsByStackId(int id);
        void Post(FlashcardDTO newFlashcard);
    }
}