using Dapper;
using Flashcards.kalsson.Interfaces;
using Flashcards.kalsson.Models;

namespace Flashcards.kalsson.Repositories;

public class StudySessionRepository : IStudySessionRepository
{
    private readonly DatabaseConfig _dbConfig;

    public StudySessionRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    public async Task<int> AddStudySessionAsync(StudySession session)
    {
        using (var connection = _dbConfig.NewConnection)
        {
            var sql = @"
                INSERT INTO StudySessions (StackId, SessionDate, Score) 
                VALUES (@StackId, @SessionDate, @Score); 
                SELECT CAST(SCOPE_IDENTITY() as int);";
            return await connection.QuerySingleAsync<int>(sql, session);
        }
    }

    public async Task<IEnumerable<StudySession>> GetStudySessionsByStackIdAsync(int stackId)
    {
        using (var connection = _dbConfig.NewConnection)
        {
            return await connection.QueryAsync<StudySession>(
                "SELECT * FROM StudySessions WHERE StackId = @StackId", new { StackId = stackId });
        }
    }

    public async Task UpdateStudySessionAsync(StudySession session)
    {
        using (var connection = _dbConfig.NewConnection)
        {
            var sql = "UPDATE StudySessions SET Score = @Score WHERE SessionId = @SessionId";
            await connection.ExecuteAsync(sql, session);
        }
    }
}