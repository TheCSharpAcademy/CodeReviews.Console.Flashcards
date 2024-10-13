using FlashcardApp.Core.Data;
using FlashcardApp.Core.Repositories;
using FlashcardApp.Core.Services;
using FlashcardApp.Core.Repositories.Interfaces;
using FlashcardApp.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using FlashcardApp.Console.Menus;

namespace FlashcardApp.Console.IoC;

public static class DependencyInjectionConfig
{
    public static void ConfigureServices(IServiceCollection services, string connectionString)
    {
        services.AddSingleton(provider => new DatabaseContext(connectionString));
        services.AddScoped<IStackRepository, StackRepository>();
        services.AddScoped<IStackService, StackService>();
        services.AddScoped<IFlashcardRepository, FlashcardRepository>();
        services.AddScoped<IFlashcardService, FlashcardService>();
        services.AddScoped<IStudySessionRepository, StudySessionRepository>();
        services.AddScoped<IStudySessionService, StudySessionService>();
        services.AddTransient<MainMenu>();
    }
}
