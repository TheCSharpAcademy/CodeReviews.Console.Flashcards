using FlashcardApp.Core.Data;
using FlashcardApp.Core.Models;
using FlashcardApp.Core.DTOs;
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
                StackId = studySession.Stack.StackId,
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

    public async Task<List<ReportingDto>> GetStudySessionsReport(string year)
    {
        try
        {
            var reportingList = new List<ReportingDto>();
            using var db = _dbContext.CreateConnection();
            string query = @"
            SELECT
                stackName,
                COALESCE([1], 0) AS Jan,
                COALESCE([2], 0) AS Feb,
                COALESCE([3], 0) AS Mar,
                COALESCE([4], 0) AS Apr,
                COALESCE([5], 0) AS May,
                COALESCE([6], 0) AS Jun,
                COALESCE([7], 0) AS Jul,
                COALESCE([8], 0) AS Aug,
                COALESCE([9], 0) AS Sep,
                COALESCE([10], 0) AS Oct,
                COALESCE([11], 0) AS Nov,
                COALESCE([12], 0) AS Dec
            FROM
                (SELECT
                    s.Name AS stackName,
                    MONTH(ss.CurrentDate) AS Month,
                    AVG(ss.Score) AS AverageScore
                 FROM
                    StudySessions ss
                 INNER JOIN
                    Stacks s ON ss.StackId = s.StackId WHERE YEAR(ss.CurrentDate) = @Year
                 GROUP BY
                    s.Name, MONTH(ss.CurrentDate)
                ) AS SourceTable
            PIVOT
                (AVG(AverageScore) FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])) AS PivotTable;";

            var reader = await db.ExecuteReaderAsync(query, new { Year = year});
            while (reader.Read())
            {
                var reportingData = new ReportingDto
                {
                    StackName = reader.GetString(0),
                    Jan = reader.GetInt32(1),
                    Feb = reader.GetInt32(2),
                    Mar = reader.GetInt32(3),
                    Apr = reader.GetInt32(4),
                    May = reader.GetInt32(5),
                    Jun = reader.GetInt32(6),
                    Jul = reader.GetInt32(7),
                    Aug = reader.GetInt32(8),
                    Sep = reader.GetInt32(9),
                    Oct = reader.GetInt32(10),
                    Nov = reader.GetInt32(11),
                    Dec = reader.GetInt32(12),
                };
                reportingList.Add(reportingData);
            }
            return reportingList;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public Task UpdateStudySession(StudySession studySession)
    {
        throw new NotImplementedException();
    }
}
