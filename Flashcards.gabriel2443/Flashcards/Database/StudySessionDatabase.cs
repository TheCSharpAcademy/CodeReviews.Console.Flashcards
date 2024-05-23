using Dapper;
using Flashcards.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Flashcards.Database;

internal class StudySessionDatabase
{
    private string connectionStr = ConfigurationManager.AppSettings.Get("ConnectionString");

    internal void AddStudySession(StudySession studySession)
    {
        var sql = @"INSERT INTO StudySession VALUES(@DateStart, @DateEnd, @Score, @QuestionCounter, @StackId)";
        using (var connection = new SqlConnection(connectionStr))
        {
            connection.Execute(sql, studySession);
        }
    }

    internal List<StudySession> GetStudySession()
    {
        var sql = @"SELECT * FROM StudySession";

        using (var connection = new SqlConnection(connectionStr))
        {
            var studySession = connection.Query<StudySession>(sql).ToList();
            return studySession;
        }
    }

    internal string GetStackName(StudySession studySession)
    {
        var sql = @$"SELECT CardstackName FROM Cardstack WHERE CardstackId = {studySession.StackId}";

        using (var connection = new SqlConnection(connectionStr))
        {
            string name = connection.ExecuteScalar<string>(sql);
            return name;
        }
    }
}