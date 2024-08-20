using Flashcards.Domain.Entities;


namespace Flashcards.Domain.Interfaces
{
    public interface IFlashcardRepository
    {
        void Add(Flashcard flashcard);
        void Update(Flashcard flashcard);
        void Delete(int flashcardId);
        Flashcard GetById(int flashcardId);

        void DeleteByStackId(int flashcardId);
        IEnumerable<Flashcard> GetByStackId(int stackId);
    }
}
