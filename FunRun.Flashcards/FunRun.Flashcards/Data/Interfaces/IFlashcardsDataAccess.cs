using FunRun.Flashcards.Data.Model;

namespace FunRun.Flashcards.Data.Interfaces;

public interface IFlashcardsDataAccess
{
    void CreateFlashcard(Flashcard flashcard);
    List<Flashcard> GetAllFlashcardsOfOneStack(Stack stack);
}