using FlashcardApp.Core.Data;
using FlashcardApp.Core.Models;
using FlashcardApp.Core.Repositories.Interfaces;
using Dapper;

namespace FlashcardApp.Core.Repositories;

public class StudySessionRepository : IStudySessionRepository
{
    private readonly DatabaseContext _dbContext;

    public StudySessionRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddStudySession(StudySession studySession)
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "INSERT INTO StudySessions (StackId, CurrentDate, Score) VALUES (@StackId, @CurrentDate, @Score)";
            await db.ExecuteAsync(query,
            new {
                StackId = studySession.stack.StackId,
                CurrentDate =   studySession.CurrentDate,
                Score=   studySession.Score
                });
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task DeleteStudySession(int id)
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "DELETE FROM StudySessions WHERE id=@Id";
            await db.ExecuteAsync(query,
            new
            {
                Id = id
            });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IEnumerable<StudySession>> GetAllStudySessions()
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "SELECT * FROM StudySessions ss ON s.stackId = ss.stackId";
            return await db.QueryAsync<StudySession>(query);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<StudySession> GetStudySessionById(int id)
    {
        try
        {
            using var db = _dbContext.CreateConnection();
            string query = "SELECT * FROM StudySessions ss LEFT JOIN Stacks s ON s.stackId = ss.stackId WHERE ss.stackId=@Id";
            var studySession =  await db.QueryFirstOrDefaultAsync<StudySession>(query, new
            {
                Id = id
            });
            if (studySession == null)
            {
                Console.Error.WriteLine("Notice: The StudySession cannot be found");
                return studySession;
            }
            return studySession;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IEnumerable<StudySession>> GetStudySessionsByStackName(string name)
    {
        using var db = _dbContext.CreateConnection();
        string query = "SELECT * FROM StudySessions ss LEFT JOIN Stacks s ON s.stackId = ss.stackId WHERE s.Name=@Name";
        var studySession = await db.QueryAsync<StudySession>(query, new
        {
            Name = name,
        });
        if (!studySession.Any() && studySession.Count() < 0)
        {
            Console.Error.WriteLine("Notice: The StudySession is empty or not found");
            return null;
        }
        return studySession;
    }

    public Task UpdateStudySession(StudySession studySession)
    {
        throw new NotImplementedException();
    }
}