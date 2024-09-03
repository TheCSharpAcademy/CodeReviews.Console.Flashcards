using System.Data.SqlClient;
using System.Data;
using CodingTrackerLibrary;
using Dapper;

namespace FlashcardsLibrary;
public class StudySessionController
{
    public static List<StudySession> GetStudySessions()
    {
        string sql = @"SELECT SS.SessionId, SS.StackId, SS.SessionDate, Stacks.Name AS StackName,
                        CAST(CAST(SUM(CASE WHEN SQ.IsCorrect = 1 THEN 1 ELSE 0 END) AS NUMERIC(14,2))
						/
                        COUNT(*) * 100 AS INT) AS Score
                        FROM StudySessions SS
                        LEFT JOIN SessionQuestion SQ ON SS.SessionId = SQ.SessionId
                        LEFT JOIN Stacks ON SS.StackId = Stacks.StackId
                        GROUP BY SS.SessionId, SS.StackId, SS.SessionDate, Stacks.Name
                        ORDER BY SS.SessionDate DESC;";
        return SqlExecutionService.GetListModels<StudySession>(sql);
    }

    public static List<StudySession> GetStudySessionsByStackId(int stackId)
    {
        string sql = @"SELECT SessionId, SessionDate
                        FROM StudySessions SS
                        LEFT JOIN Stacks ON SS.StackId = Stacks.StackId
                        WHERE SS.StackId = @StackId;";
        return SqlExecutionService.GetListModelsByKey<int, StudySession>(sql, field: "StackId", stackId);
    }

    public static StudySession GetStudySessionById(int id)
    {
        string sql = @"SELECT SS.SessionId, SS.StackId, SS.SessionDate, Stacks.Name AS StackName,
                        CAST(CAST(SUM(CASE WHEN SQ.IsCorrect = 1 THEN 1 ELSE 0 END) AS NUMERIC(14,2))
						/
                        COUNT(*) * 100 AS INT) AS Score
                        FROM StudySessions SS
                        LEFT JOIN SessionQuestion SQ ON SS.SessionId = SQ.SessionId
                        LEFT JOIN Stacks ON SS.StackId = Stacks.StackId
                        WHERE SS.SessionId = @SessionId
                        GROUP BY SS.SessionId, SS.StackId, SS.SessionDate, Stacks.Name;";
        return SqlExecutionService.GetModelByKey<int, StudySession>(sql, field: "SessionId", id);
    }

    public static int InsertStudySession(StudySession studySession)
    {
        string sql = $@"INSERT INTO dbo.StudySessions (StackId, SessionDate) 
                                        VALUES (@StackId, @SessionDate);
                        SELECT SCOPE_IDENTITY()";

        using IDbConnection connection = new SqlConnection(GlobalConfig.ConnectionString);
        connection.Open();
        int sessionId = connection.QuerySingleOrDefault<int>(sql, studySession);
        connection.Close();

        return sessionId;
    }

    public static bool DeleteStudySession(StudySession studySession)
    {
        string sql = $@"DELETE FROM dbo.StudySessions
                                WHERE SessionId = @SessionId;";

        return SqlExecutionService.ExecuteCommand<StudySession>(sql, studySession);
    }
}