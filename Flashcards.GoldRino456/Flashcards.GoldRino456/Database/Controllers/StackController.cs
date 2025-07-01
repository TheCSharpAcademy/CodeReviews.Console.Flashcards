using Dapper;
using Flashcards.GoldRino456.Database.Models;
using Utilities.GoldRino456;

namespace Flashcards.GoldRino456.Database.Controllers;
internal class StackController : IDatabaseTable<Stack>
{
    private readonly string _connectionString;

    public StackController(string connectionString)
    {
        _connectionString = connectionString;
        CreateTable();
    }

    private void CreateTable()
    {
        string createQuery = "IF OBJECT_ID(N'Stacks', N'U') IS NULL CREATE TABLE Stacks (stackId INT IDENTITY(1,1) PRIMARY KEY, stackName VARCHAR(50));";

        DatabaseUtils.ExecuteNonQueryCommand(_connectionString, createQuery);
    }

    public void CreateEntry(Stack newEntry)
    {
        string createQuery = "INSERT INTO Stacks (stackName) VALUES (@StackName);";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@StackName", newEntry.StackName);

        DatabaseUtils.ExecuteNonQueryCommand(_connectionString, createQuery, parameters);
    }

    public void DeleteEntry(int id)
    {
        string deleteQuery = "DELETE FROM Stacks WHERE stackId = @Id;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Id", id);

        DatabaseUtils.ExecuteNonQueryCommand(_connectionString, deleteQuery, parameters);
    }

    public List<Stack> ReadAllEntries()
    {
        string readQuery = "SELECT * FROM Stacks";

        return DatabaseUtils.ExecuteQueryCommand<Stack>(_connectionString, readQuery);
    }

    public List<string> ReadAllEntryNames()
    {
        string readQuery = "SELECT stackName FROM Stacks";

        return DatabaseUtils.ExecuteQueryCommand<string>(_connectionString, readQuery);
    }

    public Stack ReadEntry(int id)
    {
        string readQuery = "SELECT * FROM Stacks WHERE stackId = @Id";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Id", id);

        return DatabaseUtils.ExecuteQueryCommand<Stack>(_connectionString, readQuery, parameters).First();
    }

    public void UpdateEntry(int id, Stack updatedEntry)
    {
        string updateQuery = "UPDATE Stacks SET stackName = @StackName WHERE stackId = @Id";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Id", id);
        parameters.Add("@StackName", updatedEntry.StackName);

        DatabaseUtils.ExecuteNonQueryCommand(_connectionString, updateQuery, parameters);
    }
}
