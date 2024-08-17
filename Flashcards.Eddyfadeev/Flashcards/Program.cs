using Flashcards.Enums;
using Flashcards.Exceptions;
using Flashcards.Interfaces.Database;
using Flashcards.Interfaces.Handlers;
using Flashcards.Services;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace Flashcards;

internal static class Program
{
    private static void Main(string[] args)
    {
        var serviceCollection = ServicesConfigurator.ServiceCollection;

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mainMenuHandler = serviceProvider.GetRequiredService<IMenuHandler<MainMenuEntries>>();
        var databaseInitializer = serviceProvider.GetRequiredService<IDatabaseInitializer>();
        var databaseManager = serviceProvider.GetRequiredService<IDatabaseManager>();
        
        #if DEBUG
        DataSeed.DataSeed.ProcessRequest(databaseManager, databaseInitializer);
        #endif
        
        ShowMainMenu(mainMenuHandler);
    }
    
    private static void ShowMainMenu(IMenuHandler<MainMenuEntries> menuHandler)
    {
        AnsiConsole.MarkupLine(Messages.Messages.WelcomeMessage);
        while (true)
        {
            Console.Clear();
            try
            {
                menuHandler.HandleMenu();
            }
            catch (ReturnToMainMenuException)
            {
                // Catching the exception to return to the main menu
            }
            catch (ExitApplicationException ex)
            {
                AnsiConsole.WriteLine(ex.Message);
                break;
            }
        }
    }
}