using System.IO;
using FlashcardsAssist.DreamFXX.Data;
using FlashcardsAssist.DreamFXX.Services;
using FlashcardsAssist.DreamFXX.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlashcardsAssist.DreamFXX;
class Program
{
    static async Task Main(string[] args)
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

        var ui = serviceProvider.GetRequiredService<ConsoleUI>();
        await ui.RunAsync();
    }
}