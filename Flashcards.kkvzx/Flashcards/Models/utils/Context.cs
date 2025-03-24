using Microsoft.Extensions.Configuration;

namespace Flashcards.Models.utils;

public static class Context
{
    private static readonly IConfigurationRoot SConfig = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", false, true)
        .Build();

    public static readonly string? PostgresConnectionString =
        SConfig.GetSection("ConnectionStrings:PostgresDb").Value;

    public static readonly string? FlashcardsDbConnectionString =
        SConfig.GetSection("ConnectionStrings:FlashcardsDb").Value;

    public static readonly string? DatabaseName = SConfig.GetSection("Database").Value;
    public static readonly string? StacksTable = SConfig.GetSection("TableNames:Stacks").Value;
    public static readonly string? FlashcardsTable = SConfig.GetSection("TableNames:Flashcards").Value;
    public static readonly string? SessionsTable = SConfig.GetSection("TableNames:Sessions").Value;
}