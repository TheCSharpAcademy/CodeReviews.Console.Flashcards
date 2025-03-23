using Dapper;
using FlashcardsAssist.DreamFXX.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FlashcardsAssist.DreamFXX.Data;
public class DatabaseService
{
    private readonly string _connectionString;
    
    public DatabaseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task InitializeDatabaseAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            // Create Stacks table
            await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Stacks]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE [dbo].[Stacks] (
                        [Id] INT IDENTITY(1,1) PRIMARY KEY,
                        [Name] NVARCHAR(100) NOT NULL UNIQUE
                    )
                END
            ");
            
            // Create Flashcards table with foreign key to Stacks
            await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Flashcards]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE [dbo].[Flashcards] (
                        [Id] INT IDENTITY(1,1) PRIMARY KEY,
                        [StackId] INT NOT NULL,
                        [Front] NVARCHAR(500) NOT NULL,
                        [Back] NVARCHAR(500) NOT NULL,
                        CONSTRAINT [FK_Flashcards_Stacks] FOREIGN KEY ([StackId]) REFERENCES [dbo].[Stacks] ([Id]) ON DELETE CASCADE
                    )
                END
            ");
            
            // Create StudySessions table with foreign key to Stacks
            await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StudySessions]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE [dbo].[StudySessions] (
                        [Id] INT IDENTITY(1,1) PRIMARY KEY,
                        [StackId] INT NOT NULL,
                        [StudyDate] DATETIME NOT NULL,
                        [Score] INT NOT NULL,
                        CONSTRAINT [FK_StudySessions_Stacks] FOREIGN KEY ([StackId]) REFERENCES [dbo].[Stacks] ([Id]) ON DELETE CASCADE
                    )
                END
            ");
        }
    }

    public async Task CreateStackAsync(string stackName)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync("INSERT INTO Stacks (Name) VALUES (@Name)", new { Name = stackName });
        }
    }

    public async Task<List<Stack>> GetAllStacksAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var result = await connection.QueryAsync<Stack>("SELECT Id, Name FROM Stacks");
            return result.ToList();
        }
    }

    public async Task<Stack> GetStackByNameAsync(string stackName)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.QueryFirstOrDefaultAsync<Stack>("SELECT Id, Name FROM Stacks WHERE Name = @Name", new { Name = stackName });
        }
    }

    public async Task AddFlashcardAsync(int stackId, string front, string back)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync("INSERT INTO Flashcards (StackId, Front, Back) VALUES (@StackId, @Front, @Back)",
                new { StackId = stackId, Front = front, Back = back });
        }
    }

    public async Task<List<FlashcardDto>> GetFlashcardsForStackAsync(string stackName)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var flashcards = (await connection.QueryAsync<Flashcard>(
                "SELECT f.Id, f.Front, f.Back FROM Flashcards f " +
                "INNER JOIN Stacks s ON f.StackId = s.Id " +
                "WHERE s.Name = @Name " +
                "ORDER BY f.Id", new { Name = stackName })).ToList();

            // Re-number the flashcard IDs starting from 1
            var flashcardDtos = flashcards.Select((card, index) => new FlashcardDto
            {
                Id = index + 1,
                Front = card.Front,
                Back = card.Back
            }).ToList();

            return flashcardDtos;
        }
    }

    public async Task<List<Flashcard>> GetFlashcardsForStudyAsync(string stackName)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var result = await connection.QueryAsync<Flashcard>(
                "SELECT f.Id, f.StackId, f.Front, f.Back FROM Flashcards f " +
                "INNER JOIN Stacks s ON f.StackId = s.Id " +
                "WHERE s.Name = @Name", new { Name = stackName });
            return result.ToList();
        }
    }

    public async Task DeleteStackAsync(string stackName)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            // Due to ON DELETE CASCADE in the database, this will also delete related flashcards and study sessions
            await connection.ExecuteAsync("DELETE FROM Stacks WHERE Name = @Name", new { Name = stackName });
        }
    }

    public async Task RecordStudySessionAsync(int stackId, DateTime studyDate, int score)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync("INSERT INTO StudySessions (StackId, StudyDate, Score) VALUES (@StackId, @StudyDate, @Score)",
                new { StackId = stackId, StudyDate = studyDate, Score = score });
        }
    }

    public async Task<List<StudySession>> GetAllStudySessionsAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var result = await connection.QueryAsync<StudySession>(
                "SELECT ss.Id, ss.StackId, s.Name AS StackName, ss.StudyDate, ss.Score " +
                "FROM StudySessions ss " +
                "INNER JOIN Stacks s ON ss.StackId = s.Id " +
                "ORDER BY ss.StudyDate DESC");
            return result.ToList();
        }
    }
}
