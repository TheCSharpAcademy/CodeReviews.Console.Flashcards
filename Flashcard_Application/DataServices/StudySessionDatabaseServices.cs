using Flashcard_Application.Models;
using Microsoft.Data.SqlClient;

namespace Flashcard_Application.DataServices;

public class StudySessionDatabaseServices
{
    public static void InsertStudySession(StudySession session)
    {
        using (var connection = new SqlConnection(DatabaseConfig.dbFilePath))
        {
            connection.Open();

            using var sqlCommand = connection.CreateCommand();
            sqlCommand.Parameters.AddWithValue("@StackId", session.StackId);
            sqlCommand.Parameters.AddWithValue("@StudySessionStartTime", session.SessionStartTime);
            sqlCommand.Parameters.AddWithValue("@StudySessionEndTime", session.SessionEndTime);
            sqlCommand.Parameters.AddWithValue("@StudySessionScore", session.SessionScore);
            sqlCommand.CommandText = "INSERT INTO StudySession (StackId, StudySessionStartTime, StudySessionEndTime, StudySessionScore) VALUES (@StackId, @StudySessionStartTime, @StudySessionEndTime, @StudySessionScore)";
            sqlCommand.ExecuteNonQuery();
        }
    }

    public static List<StudySession> GetStudySession()
    {
        List<StudySession> sessionsList = new List<StudySession>();
        using (var connection = new SqlConnection(DatabaseConfig.dbFilePath))
        {
            connection.Open();
            using var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = "SELECT * FROM StudySession";

            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                sessionsList.Add(new StudySession
                {
                    SessionId = reader.GetInt32(0),
                    StackId = reader.GetInt32(1),
                    SessionStartTime = reader.GetDateTime(2),
                    SessionEndTime = reader.GetDateTime(3),
                    SessionScore = reader.GetInt32(4),
                });
            }
            return sessionsList;
        }
    }
}

