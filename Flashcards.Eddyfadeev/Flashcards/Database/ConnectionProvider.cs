using Flashcards.Interfaces.Database;
using Microsoft.Data.SqlClient;

namespace Flashcards.Database;

/// <summary>
/// Represents a connection provider for accessing the database.
/// </summary>
internal class ConnectionProvider : IConnectionProvider
{
    private readonly string _connectionString;

    public ConnectionProvider(IConfigurationProvider configurationProvider)
    {
        _connectionString = configurationProvider.GetConfiguration();
    }

    /// <summary>
    /// Gets a connection to the database.
    /// </summary>
    /// <returns>A <see cref="SqlConnection"/> object representing the database connection.</returns>
    public SqlConnection GetConnection() => new SqlConnection(_connectionString);
}