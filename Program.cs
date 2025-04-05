using Spectre.Console;
namespace Flashcards;

class Program
{
    static async Task Main()
    {
        MainMenuController mainMenuController = new();
        await mainMenuController.StartAsync();
    }
}