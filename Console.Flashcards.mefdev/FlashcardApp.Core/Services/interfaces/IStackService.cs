using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Services.Interfaces;

public interface IStackService
{
    Task<Result<string>> AddStack(string name);
    Task<Result<string>> DeleteStack(string name);
    Task<Result<string>> UpdateStack(Stack stack);
    Task<Result<Stack>> GetStackByName(string name);
    Task<Result<IEnumerable<Stack>>> GetAllStacks();
}