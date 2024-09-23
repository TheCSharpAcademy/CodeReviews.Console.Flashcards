using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Services.Interfaces;

public interface IStackService
{
    Task<Result> AddStack(string stackName);
    Task DeleteStack(int id);
    Task UpdateStack(Stack stack);
    Task<Stack> GetStackByName(string name);
    Task<IEnumerable<Stack>> GetAllStacks();
}