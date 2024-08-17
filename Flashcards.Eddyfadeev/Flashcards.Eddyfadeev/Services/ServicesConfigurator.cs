using Microsoft.Extensions.DependencyInjection;
using Flashcards.Eddyfadeev.Database;
using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Handlers;
using Flashcards.Eddyfadeev.Interfaces.Database;
using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Report;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Interfaces.View;
using Flashcards.Eddyfadeev.Interfaces.View.Factory;
using Flashcards.Eddyfadeev.Report;
using Flashcards.Eddyfadeev.Repositories;
using Flashcards.Eddyfadeev.View;
using Flashcards.Eddyfadeev.View.Factory;
using Flashcards.Eddyfadeev.View.Factory.EntriesInitializers;

namespace Flashcards.Eddyfadeev.Services;

/// <summary>
/// The ServicesConfigurator class is responsible for configuring the services used in the Flashcards application.
/// </summary>
internal static class ServicesConfigurator
{
    internal static ServiceCollection ServiceCollection { get; } = [];
    
    static ServicesConfigurator()
    {
        ConfigureServices(ServiceCollection);
    }
    
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IConfigurationProvider, ConfigurationProvider>();
        services.AddTransient<IConnectionProvider, ConnectionProvider>();
        services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
        services.AddTransient<IMenuEntries<MainMenuEntries>, MenuEntries<MainMenuEntries>>();
        services.AddTransient<IMenuEntries<StackMenuEntries>, MenuEntries<StackMenuEntries>>();
        services.AddTransient<IMenuEntries<FlashcardEntries>, MenuEntries<FlashcardEntries>>();
        services.AddTransient<IMenuEntries<StudyMenuEntries>, MenuEntries<StudyMenuEntries>>();
        services.AddTransient<IMenuEntriesInitializer<MainMenuEntries>, MainMenuEntriesInitializer>();
        services.AddTransient<IMenuEntriesInitializer<StackMenuEntries>, StacksMenuEntriesInitializer>();
        services.AddTransient<IMenuEntriesInitializer<FlashcardEntries>, FlashcardsMenuEntriesInitializer>();
        services.AddTransient<IMenuEntriesInitializer<StudyMenuEntries>, StudyMenuEntriesInitializer>();
        services.AddTransient<IEditableEntryHandler<IStack>, EditableEntryHandler<IStack>>();
        services.AddTransient<IEditableEntryHandler<IFlashcard>, EditableEntryHandler<IFlashcard>>();
        services.AddTransient<IEditableEntryHandler<IStudySession>, EditableEntryHandler<IStudySession>>();
        services.AddTransient<IMenuCommandFactory<MainMenuEntries>, MenuCommandFactory<MainMenuEntries>>();
        services.AddTransient<IMenuCommandFactory<StackMenuEntries>, MenuCommandFactory<StackMenuEntries>>();
        services.AddTransient<IMenuCommandFactory<FlashcardEntries>, MenuCommandFactory<FlashcardEntries>>();
        services.AddTransient<IMenuCommandFactory<StudyMenuEntries>, MenuCommandFactory<StudyMenuEntries>>();
        services.AddTransient<IReportGenerator, ReportGenerator>();
        
        services.AddSingleton<IDatabaseManager, DatabaseManager>();
        services.AddSingleton<IFlashcardsRepository, FlashcardsRepository>();
        services.AddSingleton<IStacksRepository, StacksRepository>();
        services.AddSingleton<IStudySessionsRepository, StudySessionsRepository>();
        services.AddSingleton<IMenuHandler<MainMenuEntries>, MenuHandler<MainMenuEntries>>();
        services.AddSingleton<IMenuHandler<StackMenuEntries>, MenuHandler<StackMenuEntries>>();
        services.AddSingleton<IMenuHandler<FlashcardEntries>, MenuHandler<FlashcardEntries>>();
        services.AddSingleton<IMenuHandler<StudyMenuEntries>, MenuHandler<StudyMenuEntries>>();
    }
}