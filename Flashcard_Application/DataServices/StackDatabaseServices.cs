using Flashcards.Models;
using Microsoft.Data.SqlClient;

namespace Flashcard_Application.DataServices;

public class StackDatabaseServices
{
    public void InsertStack(StackDto stack)
    {
        using (var connection = new SqlConnection(DatabaseConfig.dbFilePath))
        {
            connection.Open();

            using var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = "INSERT INTO Stacks (StackName, StackDescription) VALUES (@StackName, @StackDescription)";
            sqlCommand.Parameters.AddWithValue("@StackName", stack.StackName);
            sqlCommand.Parameters.AddWithValue("@StackDescription", stack.StackDescription);
            sqlCommand.ExecuteNonQuery();
        }
    }

    public static List<CardStack> GetAllStacks()
    {
        List<CardStack> list_stack = new List<CardStack>();
        using (var connection = new SqlConnection(DatabaseConfig.dbFilePath))
        {
            connection.Open();

            using var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = "SELECT * FROM Stacks";

            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                list_stack.Add(new CardStack
                {
                    StackId = reader.GetInt32(0),
                    StackName = reader.GetString(1),
                    StackDescription = reader.GetString(2)
                });
            }
        }
        return list_stack;
    }

    public static void DeleteStack(String stackName)
    {
        using (var connection = new SqlConnection(DatabaseConfig.dbFilePath))
        {
            connection.Open();
            using var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = "DELETE FROM Stacks WHERE StackName = @StackName";
            sqlCommand.Parameters.AddWithValue("@StackName", stackName);
            sqlCommand.ExecuteNonQuery();
        }
    }

    public static int GetStackID(string StackName)
    {
        int stackId = 0;
        using (var connection = new SqlConnection(DatabaseConfig.dbFilePath))
        {
            connection.Open();
            using var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = "SELECT * FROM Stacks WHERE StackName = @StackName";
            sqlCommand.Parameters.AddWithValue("@StackName", StackName);
            sqlCommand.ExecuteNonQuery();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                stackId = reader.GetInt32(0);
            }
            return stackId;
        }
    }
}
