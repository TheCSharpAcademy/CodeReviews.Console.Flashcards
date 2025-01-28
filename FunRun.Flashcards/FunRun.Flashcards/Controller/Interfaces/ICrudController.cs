using FunRun.Flashcards.Data.Model;

namespace FunRun.Flashcards.Controller.Interfaces;

public interface ICrudController
{
    void CreateStack(Stack stack);
    void DeleteStack(Stack stack);
    List<Stack> ShowAllStacks();
    void UpdateStack(Stack stack);
}