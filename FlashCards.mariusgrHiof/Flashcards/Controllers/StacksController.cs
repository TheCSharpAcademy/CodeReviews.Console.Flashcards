using Flashcards.Data;
using Flashcards.Models;

namespace Flashcards.Controllers;

public class StacksController
{
    private readonly DataAccess _dataAccess;

    public StacksController(DataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<List<Stack>> GetAllStacksAsync()
    {
        return await _dataAccess.GetAllStacksAsync();
    }

    public async Task<Stack> GetStackByIdAsync(int id)
    {
        var stack = await _dataAccess.GetStackByIdAsync(id);
        if (stack == null)
        {
            return null;
        }

        return stack;
    }

    public async Task<Stack> UpdateStackByIdAsync(int id, Stack updateStack)
    {
        var stack = await _dataAccess.UpdateStackByIdAsync(id, updateStack);
        if (stack == null)
        {
            return null;
        }

        return stack;
    }

    public async Task<Stack> DeleteStackByIdAsync(int id)
    {
        var stack = await _dataAccess.DeleteStackByIdAsync(id);
        if (stack == null) return null;

        return stack;
    }

    public async Task<Stack> AddStackAsync(Stack newStack)
    {
        var stack = await _dataAccess.AddStackAsync(newStack);
        if (stack == null) return null;

        return stack;
    }

    public async Task<Stack> GetStackByNameAsync(string stackName)
    {
        var stack = await _dataAccess.GetStackByNameAsync(stackName);
        if (stack == null) return null;

        return stack;
    }
}