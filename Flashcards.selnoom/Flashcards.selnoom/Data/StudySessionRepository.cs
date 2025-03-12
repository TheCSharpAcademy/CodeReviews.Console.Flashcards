using Flashcards.selnoom.Models;
using Microsoft.Data.SqlClient;

namespace Flashcards.selnoom.Data;

class StudySessionRepository
{
    private string connectionString;

    internal StudySessionRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    internal void CreateStudySession(int stackId, int score, DateTime sessionDate)
    {
        using var connection = new SqlConnection(connectionString);

        string query = @"INSERT INTO StudySession  (StackId, Score, SessionDate)
                         VALUES (@stackId, @score, @sessionDate)";
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@stackId", stackId);
        command.Parameters.AddWithValue("@score", score);
        command.Parameters.AddWithValue("@sessionDate", sessionDate);

        connection.Open();
        command.ExecuteNonQuery();
    }

    internal List<StudySession> GetStudySessions()
    {
        List<StudySession> studySessions = new();
        using var connection = new SqlConnection(connectionString);
        string query = @"SELECT * FROM StudySession";
        using SqlCommand command = new SqlCommand(query, connection);

        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            StudySession studySession = new StudySession
            {
                StudySessionId = reader.GetInt32(reader.GetOrdinal("StudySessionId")),
                StackId = reader.GetInt32(reader.GetOrdinal("StackId")),
                Score = reader.GetInt32(reader.GetOrdinal("Score")),
                SessionDate = reader.GetDateTime(reader.GetOrdinal("SessionDate"))
            };

            studySessions.Add(studySession);
        }

        return studySessions;
    }

    internal List<StudySessionDto> GetStudySessionDTOs()
    {
        List<StudySessionDto> sessionDTOs = new();
        using var connection = new SqlConnection(connectionString);
        string query = @"
            SELECT 
                ss.StudySessionId,
                st.StackName,
                ss.Score,
                ss.SessionDate,
                (SELECT COUNT(*) FROM Flashcard fc WHERE fc.StackId = ss.StackId) AS MaxScore
            FROM StudySession ss
            JOIN Stack st ON ss.StackId = st.StackId";
        using var command = new SqlCommand(query, connection);

        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            StudySessionDto session = new StudySessionDto
            {
                StudySessionId = reader.GetInt32(reader.GetOrdinal("StudySessionId")),
                StackName = reader.GetString(reader.GetOrdinal("StackName")),
                Score = reader.GetInt32(reader.GetOrdinal("Score")),
                SessionDate = reader.GetDateTime(reader.GetOrdinal("SessionDate")),
                MaxScore = reader.GetInt32(reader.GetOrdinal("MaxScore"))
            };

            sessionDTOs.Add(session);
        }
        return sessionDTOs;
    }
}
