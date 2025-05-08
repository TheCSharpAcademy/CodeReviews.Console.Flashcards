using Dapper;
using STUDY.ConsoleApp.Flashcards.Models;

namespace STUDY.ConsoleApp.Flashcards.Controllers;

public class StackController
{
    private readonly DatabaseHelper _databaseHelper = new();

    public void CreateStack(string stackName)
    {
        using var connection = _databaseHelper.GetConnection();
        string sql = "INSERT INTO stack(stack_name) VALUES (@StackName)";
        connection.Execute(sql, new { StackName = stackName });
    }

    public List<Stack> ListAllStacks()
    {
        using var connection = _databaseHelper.GetConnection();
        var sql = "SELECT stack_id AS Id, stack_name AS Name FROM stack ORDER BY stack_id";
        return connection.Query<Stack>(sql).ToList();
    }
    public Stack GetStackById(int stackId)
    {
        using var connection = _databaseHelper.GetConnection();
        var sql = "SELECT stack_id AS Id, stack_name AS Name FROM stack WHERE stack_id = @StackId";
        return connection.QuerySingle<Stack>(sql, new { StackId = stackId });
    }

    public void EditStackName(int stackId, string stackName)
    {
        using var connection = _databaseHelper.GetConnection();
        var sql = "UPDATE stack SET stack_name = @StackName WHERE stack_id = @StackId";
        connection.Execute(sql, new { StackName = stackName, StackId = stackId });
    }
    
    public void DeleteStack(int stackId)
    {
        using var connection = _databaseHelper.GetConnection();
        var sql = "DELETE FROM stack WHERE stack_id = @StackId";
        connection.Execute(sql, new { StackId = stackId });
    }
}