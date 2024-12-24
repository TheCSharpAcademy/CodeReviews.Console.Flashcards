using Microsoft.Extensions.DependencyInjection;
using Flashcards.Coordinators;
using Flashcards.Views;
using Flashcards.Database;
using Flashcards.Controllers;
using Flashcards.Utilities;
using Flashcards.Mappers;
using Flashcards.Models;

namespace Flashcards;

class Program
{
    static void Main(string[] args)
    {
        // Create service collection
        var services = new ServiceCollection();

        // Register services
        services.AddSingleton<MenuHandler>();
        services.AddSingleton<DatabaseManager>();
        services.AddSingleton<Validation>();
        services.AddSingleton<StackMapper>();
        services.AddSingleton<FlashcardMapper>();
        services.AddSingleton<StudySessionMapper>();
        services.AddSingleton<ListManager>();
        services.AddSingleton<StacksControllers>();
        services.AddSingleton<FlashcardsController>();
        services.AddSingleton<StudySessionController>();
        services.AddSingleton<Conversion>();
        services.AddSingleton<AppCoordinator>();

        //  Build service provider
        var serviceProvider = services.BuildServiceProvider();

        // Resolve AppCoordinator and start app
        var appCoordinator = serviceProvider.GetRequiredService<AppCoordinator>();
        appCoordinator.Start();
    }
}
