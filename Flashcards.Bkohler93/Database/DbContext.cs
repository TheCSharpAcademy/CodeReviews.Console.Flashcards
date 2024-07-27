using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Flashcards.Models;
using Models;
using System.Data.Common;

namespace Flashcards.Database;

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

                if (flashcard != null && !stackEntry.Flashcards.Any(f => f.Id == flashcard.Id))
                {
                    stackEntry.Flashcards.Add(flashcard);
                }

                if (studySession != null && !stackEntry.StudySessions.Any(s => s.Id == studySession.Id))
                {
                    stackEntry.StudySessions.Add(studySession);
                }

                return stackEntry;
            },
            splitOn: "Id,Id");

        _stackCache = [.. stackDictionary.Values.Distinct()];

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

        foreach (var flashcard in dto.Flashcards)
        {
            sql = @"
                INSERT INTO flashcards(StackId, Front, Back)
                VALUES (@StackId, @Front, @Back); 
            ";

            await conn.ExecuteAsync(sql, new { StackId = stackId, flashcard.Front, flashcard.Back });
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
        await conn.ExecuteAsync(sql, new { Id = id });
        await conn.CloseAsync();
    }


    public async Task<IEnumerable<StackInfoDto>> GetStacksInfos()
    {
        if (_stackCache.IsNullOrEmpty())
        {
            await UpdateCache();
        }

        List<StackInfoDto> dtos = [];

        foreach (var stack in _stackCache)
        {
            var newDto = new StackInfoDto(stack.Id, stack.Name);

            dtos.Add(newDto);
        }

        return dtos;
    }

    public async Task<StackInfoDto?> GetStackById(int id)
    {
        if (_stackCache.IsNullOrEmpty())
        {
            await UpdateCache();
        }

        var stack = _stackCache.FirstOrDefault(s => s.Id == id);

        if (stack == null)
        {
            return null;
        }

        return new StackInfoDto(id, stack.Name);
    }

    public async Task CreateStackFlashcardAsync(CreateStackFlashcardDto flashcard)
    {
        var sql = @"
            INSERT INTO flashcards(StackId, Front, Back)
            VALUES (@StackId, @Front, @Back); 
        ";

        using var conn = new SqlConnection(dbConnString);

        await conn.OpenAsync();
        await conn.ExecuteAsync(sql, flashcard);
        await conn.CloseAsync();

        await UpdateCache();
    }

    public async Task UpdateStackFlashcardAsync(FlashcardInfoDto updatedFlashcard)
    {
        var sql = @"
            UPDATE flashcards
            SET Front = @Front, 
                Back = @Back
            WHERE
                Id = @Id
        ";

        using var conn = new SqlConnection(dbConnString);

        await conn.OpenAsync();
        await conn.ExecuteAsync(sql, updatedFlashcard);
        await conn.CloseAsync();

        await UpdateCache();
    }

    public async Task DeleteFlashcard(FlashcardInfoDto flashcard)
    {
        var sql = @"
            DELETE FROM flashcards WHERE Id = @Id;
        ";

        using var conn = new SqlConnection(dbConnString);

        await conn.OpenAsync();
        await conn.ExecuteAsync(sql, new { Id = flashcard.Id });
        await conn.CloseAsync();

        await UpdateCache();
    }

    public async Task<FlashcardInfoDto?> GetFlashcardFromStackByIdAsync(int stackId, int flashcardId)
    {
        await EnsureCache();

        var flashcard = _stackCache.First(s => s.Id == stackId).Flashcards.First(f => f.Id == flashcardId);
        if (flashcard == null)
        {
            return null;
        }

        return new FlashcardInfoDto
        {
            Front = flashcard.Front,
            Back = flashcard.Back,
            Id = flashcard.Id,
        };
    }

    private async Task EnsureCache()
    {
        if (_stackCache.IsNullOrEmpty())
        {
            await UpdateCache();
        }
    }

    public async Task<IEnumerable<FlashcardInfoDto>> GetStackFlashcards(int stackId)
    {
        List<FlashcardInfoDto> flashcards = [];

        if (_stackCache.IsNullOrEmpty())
        {
            await UpdateCache();
        }

        var stack = _stackCache.First(s => s.Id == stackId);
        ArgumentNullException.ThrowIfNull(stack);

        foreach (var flashcard in stack.Flashcards)
        {
            var dto = new FlashcardInfoDto
            {
                Front = flashcard.Front,
                Back = flashcard.Back,
                Id = flashcard.Id,
            };

            flashcards.Add(dto);
        }
        return flashcards;
    }


    // CREATE STUDYSESSION

    // UPDATE STUDYSESSION

    // DELETE STUDYSESSION

    // GET STUDYSESSIONS
    public async Task<IEnumerable<StudySessionInfoDto>> GetStudySessions()
    {
        List<StudySessionInfoDto> studySessions = [];

        if (_stackCache.IsNullOrEmpty())
        {
            await UpdateCache();
        }

        foreach (var stack in _stackCache)
        {
            foreach (var studySession in stack.StudySessions)
            {
                var dto = new StudySessionInfoDto
                {
                    StackName = stack.Name,
                    Score = studySession.Score,
                    StudyTime = studySession.StudyTime,
                    Id = studySession.Id,
                };

                studySessions.Add(dto);
            }
        }
        return studySessions;
    }



    // GET STUDYSESSION
}
