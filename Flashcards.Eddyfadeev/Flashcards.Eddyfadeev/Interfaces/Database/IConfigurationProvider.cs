namespace Flashcards.Eddyfadeev.Interfaces.Database;

/// <summary>
/// Represents a provider for retrieving configuration data.
/// </summary>
internal interface IConfigurationProvider
{
    /// <summary>
    /// Retrieves the database connection string from the configuration.
    /// </summary>
    /// <returns>The database connection string.</returns>
    internal string GetConfiguration();

    /// <summary>
    /// Retrieves database name from the configuration
    /// </summary>
    /// <returns>The database name of type string</returns>
    internal string GetDatabaseName();
}