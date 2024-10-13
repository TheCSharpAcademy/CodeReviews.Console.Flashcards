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
    public async Task<Result<string>> AddStack(string stackName)
    {
        var existingStack = await _stackRepository.GetStackByName(stackName);
        if (existingStack != null)
        {
            return Result<string>.Failure("The stack is already exists.");
        }
        int id = GenerateRandomID();
        await _stackRepository.AddStack(new Stack { StackId = id, Name = stackName });
        return Result<string>.Success("success");
    }

    public async Task<Result<string>> DeleteStack(string name)
    {
        var stack = await _stackRepository.GetStackByName(name);
        if(stack == null)
        {
            return Result<string>.Failure("The stack is not found.");
        }
        await _stackRepository.DeleteStackByName(stack.Name);
        return Result<string>.Success("success");
    }

    public async Task<Result<IEnumerable<Stack>>> GetAllStacks()
    {
        var stacks = await _stackRepository.GetAllStacks();
        if (stacks == null || !stacks.Any())
        {
            return Result<IEnumerable<Stack>>.Failure("Notice: No stacks found.");
        }
        return Result<IEnumerable<Stack>>.Success(stacks);
    }

    public async Task<Result<Stack>> GetStackByName(string name)
    {
        var stack = await _stackRepository.GetStackByName(name);
        if (stack == null)
        {
            return Result<Stack>.Failure("The stack is not found.");
        }
        return Result<Stack>.Success(stack);
    }

    public Task<Result<string>> UpdateStack(Stack stack)
    {
        throw new NotImplementedException();
    }

    public int GenerateRandomID()
    {
        return Math.Abs(Guid.NewGuid().GetHashCode());
    }
}
