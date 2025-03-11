using Microsoft.Data.SqlClient;
using Flashcards.selnoom.Models;

namespace Flashcards.selnoom.Data;

class StackRepository
{
    private string connectionString;

    internal StackRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    internal void CreateStack(string stackname)
    {
        using var connection = new SqlConnection(connectionString);

        string query = @"INSERT INTO Stack (StackName) 
                         VALUES (@stackName)";
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@stackName", stackname);

        connection.Open();
        command.ExecuteNonQuery();
    }

    internal List<FlashcardStack> GetStacks()
    {
        List<FlashcardStack> stacks = new();
        using var connection = new SqlConnection(connectionString);
        string query = @"SELECT * FROM Stack";
        SqlCommand command = new SqlCommand(query, connection);

        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            FlashcardStack stack = new FlashcardStack
            {
                StackId = reader.GetInt32(reader.GetOrdinal("StackId")),
                StackName = reader.GetString(reader.GetOrdinal("StackName"))
            };

            stacks.Add(stack);
        }

        return stacks;
    }

    internal void UpdateStack(int stackId, string stackname)
    {
        using var connection = new SqlConnection(connectionString);

        string query = @"UPDATE Stack
                         SET StackName = @stackName
                         WHERE StackId = @stackId";
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@stackId", stackId);
        command.Parameters.AddWithValue("@stackName", stackname);

        connection.Open();
        command.ExecuteNonQuery();
    }

    internal void DeleteStack(int stackId)
    {
        using var connection = new SqlConnection(connectionString);

        string query = @"DELETE FROM Stack
                         WHERE StackId = @stackId";
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@stackId", stackId);

        connection.Open();
        command.ExecuteNonQuery();
    }
}
