
using Library.Models;

namespace DataAccess;

public interface IDataAccess
{
    public void InitDatabase();
    //public string GetConnectionString();
    public void InsertCard(CardModel flashcard);
    public void DeleteCardById(int stackId, int cardId);
    public void InsertStack(StackModel stack);
    public void DeleteStackById(int stackId);
    public List<StackModel> GetListOfStacks();
    public List<CardModel> GetCardsInStack(int id);
    public StackModel GetStackById(int stackId);
}
