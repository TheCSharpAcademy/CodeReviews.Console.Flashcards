using Dapper;
using Microsoft.Data.SqlClient;
using Models;

namespace Database;

public class DbContext(string dbConnString)
{
    private readonly string dbConnString = dbConnString;

    public async Task CreateStackAsync(CreateStackDto dto)
    {
        using var conn = new SqlConnection(dbConnString);

        var sql = @"
            INSERT INTO stacks(Name)
            VALUES (@Name);
        ";

        await conn.OpenAsync();
        await conn.ExecuteAsync(sql, dto);
        await conn.CloseAsync();
    }

    public async Task UpdateStackAsync(UpdateStackDto dto)
    {
        using var conn = new SqlConnection(dbConnString);

        var sql = @"
            UPDATE stacks
            SET Name = @Name
            WHERE Id = @Id;
        ";

        await conn.OpenAsync();
        await conn.ExecuteAsync(sql, dto);
        await conn.CloseAsync();
    }

    // DELETE STACK
    public async Task DeleteStackAsync(int id)
    {
        using var conn = new SqlConnection(dbConnString);

        var sql = "DELETE FROM stacks WHERE Id = @Id";

        await conn.OpenAsync();
        await conn.ExecuteAsync(sql, new {Id = id});
        await conn.CloseAsync();
    }

    // GET STACK



    // CREATE FLASHCARD

    // UPDATE FLASHCARD

    // DELETE FLASHCARD

    // GET FLASHCARD



    // CREATE STUDYSESSION

    // UPDATE STUDYSESSION

    // DELETE STUDYSESSION

    // GET STUDYSESSION
}
