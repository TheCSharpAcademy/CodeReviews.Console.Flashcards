using DataAccess;

namespace Library;

public class StackController
{
    private readonly IDataAccess dataAccess;

    public StackController(IDataAccess dataAccess)
    {
        this.dataAccess = dataAccess;
    }
}
