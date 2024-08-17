using Microsoft.Data.SqlClient;

namespace Flashcards.Eddyfadeev.Interfaces.Database;

/// <summary>
/// Represents a connection provider for accessing the database.
/// </summary>
internal interface IConnectionProvider
{
    /// <summary>
    /// This method is used to get a connection to the database.
    /// </summary>
    /// <returns>A <see cref="SqlConnection"/> object representing the database connection.</returns>
    internal SqlConnection GetConnection();
}