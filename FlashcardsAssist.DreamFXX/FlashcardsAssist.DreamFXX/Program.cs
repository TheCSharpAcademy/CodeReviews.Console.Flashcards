using FlashcardsAssist.DreamFXX.Data;
using FlashcardsAssist.DreamFXX.Services;
using FlashcardsAssist.DreamFXX.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace FlashcardsAssist.DreamFXX;
static class Program
{
    static async Task Main()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var serviceProvider = new ServiceCollection()
                    .AddSingleton<IConfiguration>(configuration)
                    .AddSingleton<DatabaseService>()
                    .AddSingleton<StacksService>()
                    .AddSingleton<FlashcardsService>()
                    .AddSingleton<SessionsService>()
                    .AddSingleton<StudyEngine>()
                    .AddSingleton<ReportingService>()
                    .AddSingleton<ConsoleUI>()
                    .BuildServiceProvider();

        var dbService = serviceProvider.GetRequiredService<DatabaseService>();
        await dbService.InitializeDatabaseAsync();
        AnsiConsole.MarkupLine("[green]Database initialized successfully!\nPress any key to continue.[/]");
        Console.ReadKey();

        AnsiConsole.MarkupLine("[yellow]Seeding database with sample data...[/]");
        await dbService.SeedDataAsync();
        Console.WriteLine("Database seeded successfully!");

        var ui = serviceProvider.GetRequiredService<ConsoleUI>();
        await ui.RunAsync();
    }
}