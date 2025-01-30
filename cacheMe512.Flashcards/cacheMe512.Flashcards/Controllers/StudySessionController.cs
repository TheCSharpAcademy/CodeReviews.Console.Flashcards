using cacheMe512.Flashcards.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace cacheMe512.Flashcards.Controllers;

internal class StudySessionController
{
    public IEnumerable<StudySession> GetActiveSessions()
    {
        try
        {
            using var connection = Database.GetConnection();
            return connection.Query<StudySession>(
                "SELECT Id, StackId, Date, Score FROM study_sessions WHERE Active = 1"
            ).ToList();
        }
        catch (Exception ex)
        {
            Utilities.DisplayMessage($"Error retrieving active sessions: {ex.Message}", "red");
            return Enumerable.Empty<StudySession>();
        }
    }

    public int InsertSession(StudySession session)
    {
        try
        {
            using var connection = Database.GetConnection();
            using var transaction = connection.BeginTransaction();

            var sessionId = connection.ExecuteScalar<int>(
                "INSERT INTO study_sessions (StackId, Date, Score, Active) VALUES (@StackId, @Date, @Score, 1); SELECT last_insert_rowid();",
                new { session.StackId, session.Date, session.Score },
                transaction: transaction
            );

            transaction.Commit();
            return sessionId;
        }
        catch (Exception ex)
        {
            Utilities.DisplayMessage($"Error inserting study session: {ex.Message}", "red");
            return -1;
        }
    }

    public void EndSession(int sessionId)
    {
        try
        {
            using var connection = Database.GetConnection();
            using var transaction = connection.BeginTransaction();

            connection.Execute(
                "UPDATE study_sessions SET Active = 0 WHERE Id = @Id",
                new { Id = sessionId },
                transaction: transaction
            );

            transaction.Commit();
        }
        catch (Exception ex)
        {
            Utilities.DisplayMessage($"Error ending study session: {ex.Message}", "red");
        }
    }

    public void UpdateSessionScore(int sessionId, int score)
    {
        try
        {
            using var connection = Database.GetConnection();
            using var transaction = connection.BeginTransaction();

            connection.Execute(
                "UPDATE study_sessions SET Score = @Score WHERE Id = @Id",
                new { Score = score, Id = sessionId },
                transaction: transaction
            );

            transaction.Commit();
        }
        catch (Exception ex)
        {
            Utilities.DisplayMessage($"Error updating study session score: {ex.Message}", "red");
        }
    }
}
