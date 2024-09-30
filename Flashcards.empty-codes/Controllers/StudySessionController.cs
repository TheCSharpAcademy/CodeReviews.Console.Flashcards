using Flashcards.empty_codes.Data;
using Flashcards.empty_codes.Models;
using Spectre.Console;
using System.Data.SqlClient;

namespace Flashcards.empty_codes.Controllers;

internal class StudySessionController
{
    private readonly Database database;

    public StudySessionController(Database db)
    {
        database = db;
    }

    public void InsertSession(StudySessionDTO session)
    {
        using var conn = new SqlConnection(database.connectionString);
        string insertQuery = "INSERT INTO StudySessions(SessionId, StudyDate, Score, StackId) VALUES (@SessionId, @StudyDate, @Score, @StackId)";

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@SessionId", session.SessionId);
            cmd.Parameters.AddWithValue("@StudyDate", session.StudyDate);
            cmd.Parameters.AddWithValue("@Score", session.Score);
            cmd.Parameters.AddWithValue("@StackId", session.StackId);
            cmd.ExecuteNonQuery();
        }
        catch (SqlException e)
        {
            {
                AnsiConsole.MarkupLine($"[red]Error occurred while trying to insert your flashsession\n - Details: {e.Message}[/]");
            }
        }
    }
    public List<StudySessionDTO> ViewAllSessions()
    {
        var sessions = new List<StudySessionDTO>();
        using var conn = new SqlConnection(database.connectionString);
        string readQuery = "SELECT * FROM StudySessions";

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(readQuery, conn);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                StudySessionDTO session = new StudySessionDTO
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
