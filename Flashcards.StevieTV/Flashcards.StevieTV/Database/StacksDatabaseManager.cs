using System.Data.SqlClient;
using Flashcards.StevieTV.Models;

namespace Flashcards.StevieTV.Database;

internal static class StacksDatabaseManager
{
    private static readonly string ConnectionString = Flashcards.DatabaseManager.ConnectionString;
    private const string TableName = "Stacks";

    internal static void Post(StackDTO stack)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"INSERT INTO {TableName} (Name) VALUES (N'{stack.Name}')";
                tableCommand.ExecuteNonQuery();
            }
        }
    }

    internal static void Delete(Stack stack)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"DELETE FROM {TableName} where Id = '{stack.StackId}'";
                tableCommand.ExecuteNonQuery();
            }
        }
    }

    internal static void Update(Stack stack, string newStackName)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"UPDATE {TableName} SET Name = N'{newStackName}' WHERE Id = '{stack.StackId}'";
                tableCommand.ExecuteNonQuery();
            }
        }
    }

    internal static List<Stack> GetStacks()
    {
        List<Stack> stacks = new List<Stack>();

        using (var connection = new SqlConnection(ConnectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"SELECT * FROM {TableName}";

                using (var reader = tableCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            stacks.Add(new Stack
                            {
                                StackId = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }
        }

        return stacks;
    }

    internal static Stack GetStackById(int id)
    {
        Stack stack = new Stack();

        using (var connection = new SqlConnection(ConnectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"SELECT * FROM {TableName} WHERE Id = {id}";

                using (var reader = tableCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        stack.StackId = reader.GetInt32(0);
                        stack.Name = reader.GetString(1);
                    }
                }
            }

            return stack;
        }
    }
}