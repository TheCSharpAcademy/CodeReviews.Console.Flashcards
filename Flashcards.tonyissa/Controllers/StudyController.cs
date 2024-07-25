using Flashcards.Models;
using System.Configuration;

namespace Flashcards.Controllers;

public static class StudyController
{
    private static readonly string ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;

    public static List<StudySessionDTO> GetStudySessions()
    {
        using var connection = new SqlConnection(ConnectionString);

        var sql = "SELECT startedat,endedat,stacks.name AS stackname FROM study_sessions JOIN stacks ON stackid = stacks.id;";
        var results = connection.Query<StudySessionDTO>(sql);

        return results.ToList();
    }

    public static void CreateStudySession(int stackId, DateTime startedAt)
    {
        using var connection = new SqlConnection(ConnectionString);

        var sql = "INSERT INTO study_sessions (stackid, startedat, endedat) VALUES (@StackId, @StartedAt, @EndedAt)";
        var parameters = new { StackId = stackId, StartedAt = startedAt, EndedAt = DateTime.Now };

        connection.Execute(sql, parameters);
    }
}