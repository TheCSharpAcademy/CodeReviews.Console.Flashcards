using Dapper;
using Flashcards.kalsson.Models;
using Microsoft.Data.SqlClient;

namespace Flashcards.kalsson.Data;

public class StackRepository
{
    private readonly string _connectionString;

    public StackRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<Stack> GetAllStacks()
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.Query<Stack>("SELECT * FROM Stacks");
    }

    public Stack GetStackById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.QuerySingleOrDefault<Stack>("SELECT * FROM Stacks WHERE Id = @Id", new { Id = id });
    }

    public void AddStack(Stack stack)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Execute("INSERT INTO Stacks (Name) VALUES (@Name)", stack);
    }

    public void DeleteStack(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Execute("DELETE FROM Stacks WHERE Id = @Id", new { Id = id });
    }
}