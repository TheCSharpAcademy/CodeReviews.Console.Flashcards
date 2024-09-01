using Flashcards.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace Flashcards.Tables;

public class StudySessions
{
    private static string connectionString = ConfigurationManager.AppSettings.Get("connectionString");
    public static void SaveStudySession(StudySessionDTO session)
    {
        using var connection = new SqlConnection(connectionString);
        
        string insertQuery = "INSERT INTO StudySessions (StackID, Date, Score) VALUES (@StackID, @Date, @Score)";
        
        connection.Execute(insertQuery, new { session.StackID, session.Date, session.Score });
    }

    public static IEnumerable<StudySessionDTO> GetStudySessions()
    {
        using var connection = new SqlConnection(connectionString);
        
        string query = @"
            SELECT ss.ID, ss.StackID, s.Name AS StackName, ss.Date, ss.Score
            FROM StudySessions ss
            JOIN Stacks s ON ss.StackID = s.ID";

        return connection.Query<StudySessionDTO>(query).ToList();
    }
 }
