using System.Collections.ObjectModel;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace Database;

public class DbContext(string dbConnString)
{
    private ICollection<Stack> _stackCache = [];
    private readonly string dbConnString = dbConnString;

    public async Task UpdateCache()
    {
        using var conn = new SqlConnection(dbConnString);

        var query = @"
                SELECT s.*, f.*, ss.*
                FROM Stacks s
                LEFT JOIN Flashcards f ON s.Id = f.StackId
                LEFT JOIN StudySessions ss ON s.Id = ss.StackId";

        var stackDictionary = new Dictionary<int, Stack>();

        await conn.OpenAsync();

        var stacks = await conn.QueryAsync<Stack, Flashcard, StudySession, Stack>(query,
            (stack, flashcard, studySession) =>
            {
                if (!stackDictionary.TryGetValue(stack.Id, out var stackEntry))
                {
                    stackEntry = stack;
                    stackEntry.Flashcards = [];
                    stackEntry.StudySessions = [];
                    stackDictionary.Add(stackEntry.Id, stackEntry);
                }

                if (flashcard != null)
                {
                    stackEntry.Flashcards.Add(flashcard);
                }

                if (studySession != null)
                {
                    stackEntry.StudySessions.Add(studySession);
                }

                return stackEntry;
            },
            splitOn: "Id,Id");

        _stackCache = [.. stackDictionary.Values];

        await conn.CloseAsync();
    }

    public async Task CreateStackAsync(CreateStackDto dto)
    {
        using var conn = new SqlConnection(dbConnString);

        var sql = @"
            INSERT INTO stacks(Name)
            VALUES (@Name);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
        ";

        await conn.OpenAsync();
        var stackId = await conn.QuerySingleAsync<int>(sql, dto);

        foreach(var flashcard in dto.Flashcards)
        {
            sql = @"
                INSERT INTO flashcards(StackId, Front, Back)
                VALUES (@StackId, @Front, @Back); 
            ";

            await conn.ExecuteAsync(sql, new {StackId = stackId, flashcard.Front, flashcard.Back});
        }

        await conn.CloseAsync();

        await UpdateCache();
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

    public async Task DeleteStackAsync(int id)
    {
        using var conn = new SqlConnection(dbConnString);

        var sql = "DELETE FROM stacks WHERE Id = @Id";

        await conn.OpenAsync();
        await conn.ExecuteAsync(sql, new {Id = id});
        await conn.CloseAsync();
    }


    public async Task<IEnumerable<StackInfoDto>> GetStacksInfos()
    {
        if (_stackCache.IsNullOrEmpty()) {
            await UpdateCache();
        }

        List<StackInfoDto> dtos = [];

        foreach(var stack in _stackCache) 
        {
            var newDto = new StackInfoDto(stack.Id, stack.Name);

            dtos.Add(newDto);
        }

        return dtos;
    }

    // CREATE FLASHCARD

    // UPDATE FLASHCARD

    // DELETE FLASHCARD

    // GET FLASHCARD



    // CREATE STUDYSESSION

    // UPDATE STUDYSESSION

    // DELETE STUDYSESSION

    // GET STUDYSESSION
}
