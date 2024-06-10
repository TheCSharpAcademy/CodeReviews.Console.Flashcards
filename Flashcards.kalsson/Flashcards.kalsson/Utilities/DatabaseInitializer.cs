using Microsoft.Data.SqlClient;

namespace Flashcards.kalsson.Utilities;

public static class DatabaseInitializer
{
    public static void InitializeDatabase(string connectionString)
    {
        var scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setup.sql");
        var script = File.ReadAllText(scriptPath);

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(script, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}