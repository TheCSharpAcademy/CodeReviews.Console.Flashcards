using System.Data.SQLite;
using CodingTracker.Data;
using CodingTracker.Models;
using Dapper;

namespace CodingTracker.Controllers;

public class GoalController
{
    private DataBaseService DbService { get; }
    
    public GoalController(DataBaseService dbService) => DbService = dbService;

    private List<Goal> QueryGoals(string query, object? parameters = null)
    {
        using var connection = DbService.GetConnection();
        return connection.Query<Goal>(query, parameters).ToList();
    }
    
    public List<Goal> GetAllGoals() => QueryGoals("SELECT * FROM Goals");
    public List<Goal> GetGoal(int id) => QueryGoals($"SELECT * FROM Goals WHERE Id = {id}", new {Id = id});
    
    public void InsertGoal(string startDate, string dateToComplete, string hours)
    {
        using SQLiteConnection connection = DbService.GetConnection();
        var query = $"""
                     INSERT INTO Goals(StartDate, DateToComplete, Hours)
                     VALUES
                         (@StartDate, @DateToComplete, @Hours) 
                     """;
        var parameters = new {StartDate = startDate, DateToComplete = dateToComplete, Hours = hours};
        connection.Execute(query, parameters);
    }
    
    public void UpdateGoalEndDate(int id, string endDate)
    {
        using SQLiteConnection connection = DbService.GetConnection();
        var query = $"""
                     UPDATE Goals
                     SET DateToComplete = @DateToComplete
                     WHERE Id = @Id
                     """;
        var parameters = new {Id = id, DateToComplete = endDate };
        connection.Execute(query, parameters);
    }

    public void UpdateGoalHours(int id, string hours)
    {
        using SQLiteConnection connection = DbService.GetConnection();
        var query = $"""
                     UPDATE Goals
                     SET Hours = @Hours
                     WHERE Id = @Id
                     """;
        var parameters = new {Id = id, Hours = hours };
        connection.Execute(query, parameters);
    }
    
    public void DeleteGoal(int id)
    {
        using SQLiteConnection connection = DbService.GetConnection();
        var query = $"DELETE FROM Goals WHERE Id = @Id";
        var parameters = new {Id = id};
        connection.Execute(query, parameters);
    }

    public void DeleteAllGoals()
    {
        using SQLiteConnection connection = DbService.GetConnection();
        var query = $"DELETE FROM Goals";
        connection.Execute(query);
    }
}