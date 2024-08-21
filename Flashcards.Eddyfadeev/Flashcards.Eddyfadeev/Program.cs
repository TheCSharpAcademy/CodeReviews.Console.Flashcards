using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Exceptions;
using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Services;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace Flashcards.Eddyfadeev;

internal static class Program
{
    private static void Main(string[] args)
    {
        var serviceCollection = ServicesConfigurator.ServiceCollection;

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var mainMenuHandler = serviceProvider.GetRequiredService<IMenuHandler<MainMenuEntries>>();
        
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