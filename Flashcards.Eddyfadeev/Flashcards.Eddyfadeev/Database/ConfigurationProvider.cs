using Microsoft.Extensions.Configuration;
using Database_IConfigurationProvider = Flashcards.Eddyfadeev.Interfaces.Database.IConfigurationProvider;
using IConfigurationProvider = Flashcards.Eddyfadeev.Interfaces.Database.IConfigurationProvider;

namespace Flashcards.Eddyfadeev.Database;

/// <summary>
/// Represents a provider for retrieving configuration data.
/// </summary>
internal class ConfigurationProvider : Database_IConfigurationProvider
{
    private const string AppSettingsFileName = "appsettings.json";

    /// <summary>
    /// Retrieves the database connection string from the configuration.
    /// </summary>
    /// <returns>The database connection string.</returns>
    public string GetConfiguration() => BuildConfiguration().GetSection("ConnectionStrings")["DefaultConnection"];

    public string GetDatabaseName() => BuildConfiguration().GetSection("DatabaseName")["DefaultDbName"];
    private static IConfiguration BuildConfiguration() =>
        new ConfigurationBuilder()
            .AddJsonFile(AppSettingsFileName)
            .Build();
}