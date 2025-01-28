using FunRun.Flashcards.Data.Model;

namespace FunRun.Flashcards.Data.Interfaces;

public interface IStackDataAccess
{
    void CreateStack(Stack stack);
    void DeleteStack(Stack stack);
    List<Stack> GetAllStacks();
    void UpdateStack(Stack stack);
}