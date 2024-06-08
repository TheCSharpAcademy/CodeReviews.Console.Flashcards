using Dapper;

public class StackRepository : IManageStacks
{

    private DatabaseManager _databaseManager;

    public StackRepository(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    public void CreateStack(string name)
    {
        throw new NotImplementedException();
    }

    public void DeleteStack(string name)
    {
        throw new NotImplementedException();
    }

    public List<Stack> GetStacks()
    {
        using(var conn = _databaseManager.GetConnection())
        {
            var query = "SELECT * FROM Stacks";
            return conn.Query<Stack>(query).ToList();
        }
    }
}