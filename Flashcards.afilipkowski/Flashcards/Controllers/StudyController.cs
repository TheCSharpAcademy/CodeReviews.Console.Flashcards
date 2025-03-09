using System.Configuration;
using Flashcards.Managers;
using Flashcards.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Flashcards.Controllers;

internal class StudyController
{
    private string connectionString;

    internal StudyController()
    {
        connectionString = ConfigurationManager.ConnectionStrings["dbString2"].ConnectionString;
    }

    internal void AddSession(int stackId, int score, int maxScore, DateTime date)
    {
        var sql = $"INSERT INTO StudySessions (StackId, Score, MaxScore, Date) VALUES (@StackId, @Score, @MaxScore, @Date)";
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Execute(sql, new { StackId = stackId, Score = score, MaxScore = maxScore, Date = date });
        }
    }

    internal List<StudySession> GetAllSessions()
    {
        List<StudySession> sessions = [];
        var sql = $"SELECT * FROM StudySessions ORDER BY Id";
        using (var connection = new SqlConnection(connectionString))
        {
            sessions = connection.Query<StudySession>(sql).ToList();
        }
        return sessions;
    }
}