using Spectre.Console;
namespace Flashcards;

class Program
{
    static async Task Main(string[] args)
    {
        MainMenuController mainMenuController = new();
        await mainMenuController.StartAsync();

        // await MainMenuController.BuildTables();
        // Console.Clear();

        // await ManageFlashCardsMenuController.Start();
    }
}
