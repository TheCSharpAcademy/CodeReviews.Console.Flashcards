using Flashcards.Eddyfadeev.Interfaces.Database;
using Microsoft.Data.SqlClient;

namespace Flashcards.Eddyfadeev.Database;

/// <summary>
/// Represents a connection provider for accessing the database.
/// </summary>
internal class ConnectionProvider : IConnectionProvider
{
    private const string DatabaseName = "Flashcards";
    private readonly string _connectionString;

    public ConnectionProvider(IConfigurationProvider configurationProvider)
    {
        _connectionString = configurationProvider.GetConfiguration();
        EnsureDatabaseExists();
    }

    /// <summary>
    /// Gets a connection to the database.
    /// </summary>
    /// <returns>A <see cref="SqlConnection"/> object representing the database connection.</returns>
    public SqlConnection GetConnection() => new SqlConnection(_connectionString);

    private void EnsureDatabaseExists()
    {
        using var connection = GetConnection();
        connection.Open();

        const string commandText = 
            $"""
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{DatabaseName}')
                    BEGIN
                        CREATE DATABASE [{DatabaseName}];
                    END
            """;

        using var command = new SqlCommand(commandText, connection);
        command.ExecuteNonQuery();
    }
}