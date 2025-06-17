namespace Database;
using Microsoft.Data.SqlClient;

public class DbContext
{

    public SqlConnection DBConnection()
    {
        string? connectionString = System.Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("DB_CONNECTION_STRING environment variable is not set");
        }
        return new SqlConnection(connectionString);
    }

    public void ConnectionStatus()
    {
        try
        {
            using (var connection = this.DBConnection())
            {
                connection.Open();
                Console.WriteLine("Db connected successfully.");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"SQL Error {ex.Number}: {ex.Message} (State: {ex.State}, Severity: {ex.Class})");
        }
    }
}
