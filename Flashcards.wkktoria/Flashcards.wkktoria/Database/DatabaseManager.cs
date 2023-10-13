using System.Data;
using Microsoft.Data.SqlClient;

namespace Flashcards.wkktoria.Database;

public class DatabaseManager
{
    private readonly SqlConnection _connection;
    private readonly string _databaseName;

    public DatabaseManager(string connectionString, string databaseName)
    {
        _connection = new SqlConnection(connectionString);
        _databaseName = databaseName;
    }

    public void Initialize()
    {
        try
        {
            _connection.Open();

            var query = $"""
                         IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'{_databaseName}')
                         BEGIN
                            CREATE DATABASE {_databaseName};
                         END;
                         """;
            var command = new SqlCommand(query, _connection);

            command.ExecuteNonQuery();
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to initialize database.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }
    }
}