using FlashCards.kwm0304.Config;
using FlashCards.kwm0304.Models;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace FlashCards.kwm0304.Repositories;

public class StackRepository
{
    private readonly string _connString;
    public StackRepository()
    {
        _connString = AppConfiguration.GetConnectionString("DefaultConnection");
    }

    internal async Task<Stack> GetStackAsync(int stackId)
    {
        var sql = "SELECT StackName FROM Stacks WHERE StackId = @Id";
        using var connection = new SqlConnection(_connString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", stackId);
        try
        {
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var stack = new Stack(
                    reader["StackName"].ToString() ?? throw new InvalidOperationException("Stack name cannot be null")
                )
                {
                    StackId = Convert.ToInt32(reader["StackId"])
                };
                return stack;
            }
            else
            {
                throw new InvalidOperationException("Stack not found");
            }
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            throw;
        }
    }

    internal async Task<List<Stack>> GetAllStacksAsync()
    {
        List<Stack> stacks = [];
        var sql = "SELECT * FROM Stacks";
        using var connection = new SqlConnection(_connString);
        await connection.OpenAsync();
        using var command = new SqlCommand(sql, connection);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var stack = new Stack(
                reader["StackName"].ToString() ?? throw new InvalidOperationException("Stack name cannot be null")
            )
            {
                StackId = Convert.ToInt32(reader["StackId"])
            };
            stacks.Add(stack);
        }
        return stacks;
    }

    public async Task<int> CreateStackAsync(string name)
    {
        var sql = "INSERT INTO Stacks (StackName) VALUES (@StackName); SELECT SCOPE_IDENTITY();";
        using var connection = new SqlConnection(_connString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@StackName", name);
        try
        {
            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            AnsiConsole.WriteLine("Stack created successfully");
            return Convert.ToInt32(result);
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            throw;
        }
    }

    internal async Task UpdateStackAsync(int id, string name)
    {
        var sql = "UPDATE Stacks SET StackName = @Name WHERE StackId = @Id";
        using var connection = new SqlConnection(_connString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@StackName", name);
        command.Parameters.AddWithValue("@Id", id);
        try
        {
            await connection.OpenAsync();
            command.ExecuteNonQuery();
            AnsiConsole.WriteLine($"Stack name updated to {name}");
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
        }
    }

    internal async Task DeleteStackAsync(int id)
    {
        var sql = "DELETE FROM Stacks WHERE StackId = @Id";
        using var connection = new SqlConnection(_connString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);
        try
        {
            await connection.OpenAsync();
            command.ExecuteNonQuery();
            AnsiConsole.WriteLine("Stack deleted successfully");
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
        }
    }
}