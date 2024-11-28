using FlashCards.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace FlashCards.Controllers;

public class StackController(DatabaseService DbService)
{
    private DatabaseService DbService { get; } = DbService;
    
    public List<StackDto> GetAllStackNames() => ExecuteDtoQuery("SELECT name FROM Stacks");
    
    public void InsertStack(string name)
    {
        var query = @"INSERT INTO Stacks (Name) VALUES (@Name)";
        var parameters = new { Name = name };
        ExecuteNonQuery(query, parameters);
    }
    
    public void UpdateStack(string oldStackName, string newStackName)
    {
        var query = @"UPDATE Stacks SET Name = @NewStackName WHERE Name = @OldStackName";
        var parameters = new { newStackName = newStackName, OldStackName = oldStackName };
        ExecuteNonQuery(query, parameters);
    }
  
    public void DeleteStack(string name)
    {
        var query = @"DELETE FROM Stacks WHERE Name = @Name";
        var parameters = new { Name = name };
        ExecuteNonQuery(query, parameters);
    }
    
    internal List<Stack> GetAllStacks() => ExecuteStackQuery("SELECT * FROM Stacks");
    private List<StackDto> ExecuteDtoQuery(string query, object? parameters = null)
    {
        using SqlConnection connection = DbService.GetConnection();
        return connection.Query<StackDto>(query, parameters).ToList();
    }

    private void ExecuteNonQuery(string query, object? parameters = null)
    {
        using SqlConnection connection = DbService.GetConnection();
        connection.Execute(query, parameters);
    }

    private List<Stack> ExecuteStackQuery(string query, object? parameters = null)
    {
        using SqlConnection connection = DbService.GetConnection();
        return connection.Query<Stack>(query, parameters).ToList();
    }
}