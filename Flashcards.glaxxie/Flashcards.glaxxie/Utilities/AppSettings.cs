using Flashcards.glaxxie.DTO;
using Microsoft.Extensions.Configuration;

namespace Flashcards.glaxxie.Utilities;

internal sealed class AppSettings
{
    private static readonly IConfigurationRoot _config;

    static AppSettings()
    {
        _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
    }

    internal static CardStyleDto StyleGetter(string style) =>
        style switch
        {
            "Vertical" => _config.GetSection("AppSettings:Card:Style:Vertical").Get<CardStyleFromConfig>()!.ToCardStyleDto(),
            _ => _config.GetSection("AppSettings:Card:Style:Horizontal").Get<CardStyleFromConfig>()!.ToCardStyleDto()
        };
    
    internal static string MasterConnectionString => _config.GetConnectionString("Master")!;
    internal static string DefaultConnectionString => _config.GetConnectionString("Default")!;

    internal static string CardsTable => _config["Database:Tables:Cards"]!;
    internal static string SessionsTable => _config["Database:Tables:Sessions"]!;
    internal static string StacksTable => _config["Database:Tables:Stacks"]!;

    internal static string CardLayout => _config["AppSettings:Card:Layout"]!;
    internal static string CardBorder => _config["AppSettings:Card:Border"]!;
}