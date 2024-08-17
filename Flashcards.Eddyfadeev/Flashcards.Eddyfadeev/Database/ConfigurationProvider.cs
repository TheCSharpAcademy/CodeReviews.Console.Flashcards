using Microsoft.Extensions.Configuration;
using IConfigurationProvider = Flashcards.Eddyfadeev.Interfaces.Database.IConfigurationProvider;

namespace Flashcards.Eddyfadeev.Database;

/// <summary>
/// Represents a provider for retrieving configuration data.
/// </summary>
internal class ConfigurationProvider : IConfigurationProvider
{
    private const string AppSettingsFileName = "appsettings.json";

    /// <summary>
    /// Retrieves the database connection string from the configuration.
    /// </summary>
    /// <returns>The database connection string.</returns>
    public string GetConfiguration() => BuildConfiguration().GetSection("ConnectionStrings")["DefaultConnection"];

    private static IConfiguration BuildConfiguration() =>
        new ConfigurationBuilder()
            .AddJsonFile(AppSettingsFileName)
            .Build();
}