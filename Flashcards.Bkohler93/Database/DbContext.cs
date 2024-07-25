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
            INSERT INTO stacks(name)
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
            SET name = @Name
            WHERE id = @Id;
        ";

        await conn.OpenAsync();
        await conn.ExecuteAsync(sql, dto);
        await conn.CloseAsync();
    }

    // DELETE STACK

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
