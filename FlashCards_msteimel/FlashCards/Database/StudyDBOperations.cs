using FlashCards.Models;
using Microsoft.Data.SqlClient;

namespace FlashCards.Database;

internal static class StudyDBOperations
{
    private static readonly string studySessionTableName = "StudySessions";

    internal static void AddSession(StudySession studySession)
    {
        using (SqlConnection connection = new SqlConnection(DatabaseCreation.dbConnectionString))
        {
            connection.Open();
            string insertQuery = $"INSERT INTO {studySessionTableName} (Date, Score, StackId, StackName) VALUES (@Date, @Score, @StackId, @StackName)";
            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@Date", studySession.Date);
                command.Parameters.AddWithValue("@Score", studySession.Score);
                command.Parameters.AddWithValue("@StackId", studySession.StackId);
                command.Parameters.AddWithValue("@StackName", studySession.StackName);
                command.ExecuteNonQuery();
            }
        }
    }

    internal static List<StudySession> GetStudySessions()
    {
        var sessions = new List<StudySession>();

        using (SqlConnection connection = new SqlConnection(DatabaseCreation.dbConnectionString))
        {
            connection.Open();
            string selectQuery = $"SELECT * FROM {studySessionTableName}";
            using (SqlCommand command = new SqlCommand(selectQuery, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var session = new StudySession
                        {
                            Date = reader.GetDateTime(1),
                            Score = reader.GetString(2),
                            StackName = reader.GetString(3)
                        };
                        sessions.Add(session);
                    }
                }
            }
        }

        return sessions;
    }
}
