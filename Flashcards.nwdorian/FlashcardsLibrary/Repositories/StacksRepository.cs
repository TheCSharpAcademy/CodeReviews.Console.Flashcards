using Dapper;
using FlashcardsLibrary.Models;
using Microsoft.Data.SqlClient;

namespace FlashcardsLibrary.Repositories;
public class StacksRepository : IStacksRepository
{
    private readonly string? connectionString;

    public StacksRepository()
    {
        connectionString = AppConfig.GetFullConnectionString();
    }
    public async Task<IEnumerable<Stack>> GetAllAsync()
    {
        using var connection = new SqlConnection(connectionString);

        connection.Open();

        var getAllSql = "SELECT Id, Name FROM Stack";

        return await connection.QueryAsync<Stack>(getAllSql);
    }
    public async Task AddAsync(Stack stack)
    {
        using var connection = new SqlConnection(connectionString);

        connection.Open();

        var insertSql = "INSERT INTO Stack (Name) VALUES (@Name)";

        await connection.ExecuteAsync(insertSql, stack);
    }

    public async Task DeleteAsync(Stack stack)
    {
        using var connection = new SqlConnection(connectionString);

        connection.Open();

        var deleteSql = "DELETE FROM Stack WHERE Id = @Id";

        await connection.ExecuteAsync(deleteSql, stack);
    }

    public async Task UpdateAsync(Stack stack)
    {
        using var connection = new SqlConnection(connectionString);

        connection.Open();

        var updateSQL = "UPDATE Stack SET Name = @Name WHERE Id = @Id";

        await connection.ExecuteAsync(updateSQL, stack);
    }

    public async Task<bool> StackNameExistsAsync(string name)
    {
        using var connection = new SqlConnection(connectionString);

        connection.Open();

        var checkNameSql = "SELECT TOP 1 COUNT(*) FROM Stack WHERE Name = @Name";

        return await connection.ExecuteScalarAsync<bool>(checkNameSql, new { Name = name });
    }
}
