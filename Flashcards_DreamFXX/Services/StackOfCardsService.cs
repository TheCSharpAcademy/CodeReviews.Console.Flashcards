using Flashcards.DreamFXX.Data;
using Flashcards.DreamFXX.Models;

namespace Flashcards_DreamFXX.Services;

public interface IStackOfCardsService
{
    StackOfCards? DisplayExistingStacks { get; set; }
    void CreateCardStack();
    void EditStack();
    void DeleteStack();
}

public class StackOfCardsService : IStackOfCardsService
{
    private readonly DbManager _dbManager;

    public StackOfCardsService(DbManager dbManager)
    {
        _dbManager = dbManager;
    }

    public StackOfCards? DisplayExistingStacks { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void CreateCardStack()
    {
        throw new NotImplementedException();
    }

    public void DeleteStack()
    {
        throw new NotImplementedException();
    }

    public void EditStack()
    {
        throw new NotImplementedException();
    }
}
