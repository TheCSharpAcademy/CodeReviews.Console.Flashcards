using Flashcards.empty_codes.Data;
using Flashcards.empty_codes.Models;
using Spectre.Console;
using System.Data.SqlClient;

namespace Flashcards.empty_codes.Controllers;

internal class StudySessionController
{
    public void InsertSession(StudySessionDto session)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        string insertQuery = "INSERT INTO StudySessions(StudyDate, Score, StackId) VALUES (@StudyDate, @Score, @StackId)";

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@StudyDate", session.StudyDate);
            cmd.Parameters.AddWithValue("@Score", session.Score);
            cmd.Parameters.AddWithValue("@StackId", session.StackId);
            cmd.ExecuteNonQuery();
        }
        catch (SqlException e)
        {
            AnsiConsole.MarkupLine($"[red]Error occurred while trying to insert your session\n - Details: {e.Message}[/]"); 
        }
    }
    public List<StudySessionDto> ViewAllSessions()
    {
        var sessions = new List<StudySessionDto>();
        using var conn = new SqlConnection(Database.ConnectionString);
        string readQuery = "SELECT * FROM StudySessions";

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(readQuery, conn);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                StudySessionDto session = new StudySessionDto
                {
                    SessionId = reader.GetInt32(0),
                    StudyDate = reader.GetDateTime(1),
                    Score = reader.GetString(2),
                    StackId = reader.GetInt32(3)
                };
                sessions.Add(session);
            }
        }
        catch (SqlException e)
        {
            AnsiConsole.MarkupLine($"[red]Error occurred while trying to access your sessions\n - Details: {e.Message}[/]");
        }
        return sessions;
    }
}