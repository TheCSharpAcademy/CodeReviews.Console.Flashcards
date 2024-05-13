using Microsoft.Extensions.Configuration;

namespace FlashcardsLibrary;
internal class AppConfig
{
    private static IConfiguration? _iconfiguration;

    static AppConfig()
    {
        GetAppSettingsFile();
    }

    public static void GetAppSettingsFile()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
            
        _iconfiguration = builder.Build();
    }

    public static string GetDbConnectionString()
    {
        return _iconfiguration?.GetConnectionString("Default") ?? throw new Exception("Connection string error!");
    }

    public static string GetFullConnectionString()
    {
        return _iconfiguration?.GetConnectionString("Flashcards") ?? throw new Exception("Connection string error!");
    }
}
