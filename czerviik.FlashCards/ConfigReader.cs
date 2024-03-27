using Microsoft.Extensions.Configuration;

namespace FlashCards;

public class ConfigReader
{
    public IConfigurationRoot Configuration { get; }

    public ConfigReader()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) //presence of the file is not optional, file reloads on change
            .Build();
    }

    #nullable enable
    public string GetConnectionString()
    {
        string? configString = Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(configString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection'is not configured.");
        }
        return configString;
    }

    public string GetFileNameString()
    {
        string? configString = Configuration.GetConnectionString("FileName");
        if (string.IsNullOrEmpty(configString))
        {
            throw new InvalidOperationException("Connection string 'FileName'is not configured.");
        }
        return configString;
    }
}