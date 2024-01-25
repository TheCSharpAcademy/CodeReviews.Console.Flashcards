using DataAccess;

namespace Library;

public class CardController
{
    private readonly IDataAccess dataAccess;

    public CardController(IDataAccess dataAccess)
    {
        this.dataAccess = dataAccess;
    }
}
