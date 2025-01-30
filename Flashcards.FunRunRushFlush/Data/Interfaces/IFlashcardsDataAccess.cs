using Flashcards.FunRunRushFlush.Data.Model;

namespace Flashcards.FunRunRushFlush.Data.Interfaces;

public interface IFlashcardsDataAccess
{
    void CreateFlashcard(Flashcard flashcard);
    List<Flashcard> GetAllFlashcardsOfOneStack(Stack stack);
}