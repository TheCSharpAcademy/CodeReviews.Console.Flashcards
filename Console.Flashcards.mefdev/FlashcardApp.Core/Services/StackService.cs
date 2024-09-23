using FlashcardApp.Core.Repositories.Interfaces;
using FlashcardApp.Core.Services.Interfaces;
using FlashcardApp.Core.Models;

namespace FlashcardApp.Core.Services;

public class StackService : IStackService
{
    private readonly IStackRepository _stackRepository;

    public StackService(IStackRepository stackRepository)
    {
        _stackRepository = stackRepository;
    }
    public async Task<Result> AddStack(string stackName)
    {
        var existingStack = await _stackRepository.GetStackByName(stackName);
        if (existingStack != null)
        {
            return Result.Failure("The stack is already exists.");
        }
        int id = GenerateRandomID();
        await _stackRepository.AddStack(new Stack { StackId = id, Name = stackName });
        return Result.Success();
    }

    public Task DeleteStack(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Stack>> GetAllStacks()
    {
        throw new NotImplementedException();
    }

    public Task<Stack> GetStackByName(string name)
    {
        throw new NotImplementedException();
    }

    public Task UpdateStack(Stack stack)
    {
        throw new NotImplementedException();
    }

    private int GenerateRandomID()
    {
        return Math.Abs(Guid.NewGuid().GetHashCode());
    }
}