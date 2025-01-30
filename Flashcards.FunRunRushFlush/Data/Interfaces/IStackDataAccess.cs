using Flashcards.FunRunRushFlush.Data.Model;

namespace Flashcards.FunRunRushFlush.Data.Interfaces;

public interface IStackDataAccess
{
    void CreateStack(Stack stack);
    void DeleteStack(Stack stack);
    List<Stack> GetAllStacks();
    void UpdateStack(Stack stack);
}