using Microsoft.Extensions.Configuration;

namespace Flashcards;

internal static class Configuration
{
    private static IConfiguration configuration;
    
    internal static string GetConnectionString()
    {
        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json")
            .Build();
        
        return configuration["connectionString"];
    }
}