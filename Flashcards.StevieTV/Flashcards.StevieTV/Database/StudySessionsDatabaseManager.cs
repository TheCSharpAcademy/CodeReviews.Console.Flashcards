using System.Data.SqlClient;
using Flashcards.StevieTV.Models;

namespace Flashcards.StevieTV.Database;

internal static class StudySessionsDatabaseManager
{
    private static readonly string connectionString = Flashcards.DatabaseManager.connectionString;
    private static readonly string tableName = "StudySessions";

    internal static void Post(StudySession studySession)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"INSERT INTO {tableName} (Date, StackId, Score, QuantityTested) VALUES (@date, {studySession.Stack.StackId}, {studySession.Score}, {studySession.QuantityTested})";
                tableCommand.Parameters.AddWithValue("@date", studySession.DateTime);
                tableCommand.ExecuteNonQuery();
            }
        }
    }
    


    internal static List<StudySession> GetStudySessions()
    {
        List<StudySession> studySessions = new List<StudySession>();
    
        using (var connection = new SqlConnection(connectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"SELECT Date, StackId, Score, QuantityTested FROM {tableName} ORDER BY Date";
    
                using (var reader = tableCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            studySessions.Add(new StudySession
                            {
                                DateTime= reader.GetDateTime(0),
                                Stack = StacksDatabaseManager.GetStackById(reader.GetInt32(1)),
                                Score = reader.GetInt32(2),
                                QuantityTested = reader.GetInt32(3)
                            });
                        }
                    }
                }
            }
        }
        
        return studySessions;
    }


}