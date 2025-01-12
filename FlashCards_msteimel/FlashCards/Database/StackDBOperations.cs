using FlashCards.Models;
using Microsoft.Data.SqlClient;

namespace FlashCards.Database
{
    internal static class StackDBOperations
    {
        private static readonly string dataBaseName = "FlashCardsDB";
        private static readonly string dbConnectionString = $"Server=LAPTOP-LFCOM607;Database={dataBaseName};Integrated Security=true;TrustServerCertificate=True;";
        private static readonly string stackTableName = "Stacks";

        internal static void AddStack(Stack stack)
        {
            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                string insertQuery = $"INSERT INTO {stackTableName} (Name) VALUES (@Name)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", stack.Name);
                    command.ExecuteNonQuery();
                }
            }
        }

        internal static List<Stack> GetStacks()
        {
            var stacks = new List<Stack>();

            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                string selectQuery = $"SELECT * FROM {stackTableName}";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var stack = new Stack
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            };
                            stacks.Add(stack);
                        }
                    }
                }
            }

            return stacks;
        }

        internal static void DeleteStack(int id)
        {
            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                string deleteQuery = $"DELETE FROM {stackTableName} WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        internal static void RenameStack(int id, string newName)
        {
            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                string updateQuery = $"UPDATE {stackTableName} SET Name = @NewName WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@NewName", newName);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        internal static int GetStackIDByName(string name)
        {

            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                connection.Open();
                string selectQuery = $"SELECT Id FROM {stackTableName} WHERE Name = @name";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                    }
                }
            }

            return -1;
        }

    }
}
