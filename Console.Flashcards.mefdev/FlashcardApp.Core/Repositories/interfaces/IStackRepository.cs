using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Repositories.Interfaces;

public interface IStackRepository
{
    Task AddStack(Stack stack);
    Task DeleteStack(int id);
    Task DeleteStackByName(string name);
    Task UpdateStack(Stack stack);
    Task<Stack> GetStackByName(string name);
    Task<IEnumerable<Stack>> GetAllStacks();
}
