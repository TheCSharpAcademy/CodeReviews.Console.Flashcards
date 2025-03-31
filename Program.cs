using Spectre.Console;
namespace Flashcards;

class Program
{
    static readonly int ERRORCODETABLEEXISTS = -2146232060;
    static async Task Main(string[] args)
    {
        Console.Clear();
        DataBaseManager.Start();

        await DataBaseManager.BuildTable("stacks",
        [
            "Id INTEGER PRIMARY KEY",
            "Name TEXT"
        ]);

        await DataBaseManager.BuildTable("flash_cards",
        [
            "Stacks_Id INTEGER NOT NULL",
            "FOREIGN KEY (Stacks_Id) REFERENCES stacks (Id)",
            "Id INTEGER PRIMARY KEY",
            "Front TEXT",
            "Back TEXT"
        ]);

        await MainMenuController.Start();

        // List<Stack> results = await DataBaseManager.GetAllLogs<Stack>("stacks");
        // DisplayOther.DisplayTable(results);
    }
}
