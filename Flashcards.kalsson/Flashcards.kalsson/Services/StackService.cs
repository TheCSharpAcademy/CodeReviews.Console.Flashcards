using Flashcards.kalsson.Data;
using Flashcards.kalsson.Models;

namespace Flashcards.kalsson.Services;

public class StackService
{
    private readonly StackRepository _stackRepository;

    public StackService(StackRepository stackRepository)
    {
        _stackRepository = stackRepository;
    }

    public IEnumerable<Stack> GetAllStacks()
    {
        return _stackRepository.GetAllStacks();
    }

    public Stack GetStackById(int id)
    {
        return _stackRepository.GetStackById(id);
    }

    public void AddStack(Stack stack)
    {
        _stackRepository.AddStack(stack);
    }

    public void DeleteStack(int id)
    {
        _stackRepository.DeleteStack(id);
    }
}