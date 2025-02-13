using Flashcards.Dreamfxx.Data;
using Flashcards.Dreamfxx.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceCollection = new ServiceCollection();
ConfigureServices(serviceCollection);

var serviceProvider = serviceCollection.BuildServiceProvider();

var stacksService = serviceProvider.GetService<StacksService>();
var flashcardsService = serviceProvider.GetService<FlashcardsService>();
var sessionsService = serviceProvider.GetService<SessionsService>();

// Příklad použití služby
// await stacksService.SomeMethodAsync();

void ConfigureServices(IServiceCollection services)
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    services.AddSingleton<IConfiguration>(configuration);

    var connectionString = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    services.AddSingleton(new DatabaseManager(connectionString));
    services.AddTransient<StacksService>();
    services.AddTransient<FlashcardsService>();
    services.AddTransient<SessionsService>();

    services.AddLogging(configure => configure.AddConsole());
}
