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
                tableCommand.CommandText = $"INSERT INTO StudySessions (Date, StackId, Score) VALUES (@date, {studySession.Stack.StackId}, {studySession.Score})";
                tableCommand.Parameters.AddWithValue("@date", studySession.DateTime);
                tableCommand.ExecuteNonQuery();
            }
        }
    }
    


    // internal static List<Stack> GetStacks()
    // {
    //     List<Stack> stacks = new List<Stack>();
    //
    //     using (var connection = new SqlConnection(connectionString))
    //     {
    //         using (var tableCommand = connection.CreateCommand())
    //         {
    //             connection.Open();
    //             tableCommand.CommandText = "SELECT * FROM Stacks";
    //
    //             using (var reader = tableCommand.ExecuteReader())
    //             {
    //                 if (reader.HasRows)
    //                 {
    //                     while (reader.Read())
    //                     {
    //                         stacks.Add(new Stack
    //                         {
    //                             StackId = reader.GetInt32(0),
    //                             Name = reader.GetString(1)
    //                         });
    //                     }
    //                 }
    //             }
    //         }
    //     }
    //
    //     return stacks;
    // }


}