using System.Data.SqlClient;
using Flashcards.StevieTV.Models;

namespace Flashcards.StevieTV.Database;

internal static class StacksDatabaseManager
{
    private static readonly string connectionString = Flashcards.DatabaseManager.connectionString;
    private static readonly string tableName = "Stacks";

    internal static void Post(StackDTO stack)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"INSERT INTO {tableName} (Name) VALUES (N'{stack.Name}')";
                tableCommand.ExecuteNonQuery();
            }
        }
    }
    
    internal static void Delete(Stack stack)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"DELETE FROM {tableName} where Id = '{stack.StackId}'";
                tableCommand.ExecuteNonQuery();
            }
        }
    }
    
    internal static void Update(Stack stack, string newStackName)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"UPDATE {tableName} SET Name = N'{newStackName}' WHERE Id = '{stack.StackId}'";
                tableCommand.ExecuteNonQuery();
            }
        }
    }

    internal static List<Stack> GetStacks()
    {
        List<Stack> stacks = new List<Stack>();

        using (var connection = new SqlConnection(connectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"SELECT * FROM {tableName}";

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

    internal static Stack GetStackByName(string stackName)
    {
        Stack stack = new Stack();

        using (var connection = new SqlConnection(connectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"SELECT * FROM {tableName} WHERE Name = '{stackName}'";

                using (var reader = tableCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        stack.StackId = reader.GetInt32(0);
                        stack.Name = reader.GetString(1);
                    }

                    return stack;
                }
            }
        }
    }
}