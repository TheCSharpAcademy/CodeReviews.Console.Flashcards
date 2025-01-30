using Flashcards.FunRunRushFlush.Data.Model;

namespace Flashcards.FunRunRushFlush.Controller.Interfaces;

public interface ICrudController
{
    void CreateFlashcard(Flashcard flashcard);
    void CreateStack(Stack stack);
    void DeleteStack(Stack stack);
    List<Flashcard> ShowAllFlashcardsOfSelectedStack(Stack stack);
    List<Stack> ShowAllStacks();
    void UpdateStack(Stack stack);
}