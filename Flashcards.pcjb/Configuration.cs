namespace Flashcards;

using Microsoft.Extensions.Configuration;

internal class Configuration
{
    IConfigurationRoot config;

    public Configuration()
    {
        config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .Build();
    }

    internal string? DatabaseConnectionString
    {
        get
        {
            var connString = config["DatabaseConnectionString"];
            if (!String.IsNullOrEmpty(connString))
            {
                connString = connString.Replace("{DatabaseUserID}", config["DatabaseUserID"]);
                connString = connString.Replace("{DatabasePassword}", config["DatabasePassword"]);
            }
            return connString;
        }
    }
}