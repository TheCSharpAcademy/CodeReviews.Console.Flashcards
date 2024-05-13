using Dapper;
using FlashcardsLibrary.Models;
using Microsoft.Data.SqlClient;

namespace FlashcardsLibrary.Repositories;
public class StudySessionsRepository : IStudySessionsRepository
{
    private readonly string? connectionString;

    public StudySessionsRepository()
    {
        connectionString = AppConfig.GetFullConnectionString();
    }
    public async Task<IEnumerable<StudySession>> GetAllAsync()
    {
        using var connection = new SqlConnection(connectionString);

        var getAllSql = """
                            SELECT ss.id, ss.date, ss.score, ss.stackid, s.name
                            FROM   studysession AS ss
                                   JOIN stack AS s
                                     ON ss.stackid = s.id
                            """;

        return await connection.QueryAsync<StudySession, Stack, StudySession>(getAllSql, (studysession, stack) =>
        {
            studysession.Stack = stack;
            return studysession;
        },
        splitOn: "StackId");
    }
    public async Task AddAsync(StudySession session)
    {
        using var connection = new SqlConnection(connectionString);

        var insertSql = "INSERT INTO StudySession (StackId, Date, Score) VALUES (@StackId, @Date, @Score)";

        await connection.ExecuteAsync(insertSql, session);
    }
}
