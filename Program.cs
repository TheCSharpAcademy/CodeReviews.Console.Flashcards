using Spectre.Console;
namespace Flashcards;

class Program
{
    static readonly int ERRORCODETABLEEXISTS = -2146232060;
    static async Task Main(string[] args)
    {
        await MainMenuController.Start();
        // await MainMenuController.BuildTables();
        // Console.Clear();
        // List<Stack> stackSet = await DataBaseManager<Stack>.GetLogs();
        // ManageFlashCardsMenuController.currentStack = GetInput.Selection(stackSet);

        // await ManageFlashCardsMenuController.ViewCardsAsync();
    }
}
