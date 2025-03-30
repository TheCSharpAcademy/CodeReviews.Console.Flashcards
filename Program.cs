using Spectre.Console;

namespace Flashcards;


class Program
{
    static readonly int ERRORCODETABLEEXISTS = -2146232060;
    static async Task Main(string[] args)
    {
        Console.Clear();
        DataBaseManager<Stack>.Start();
        DataBaseManager<FlashcardDTO>.Start();

        await DataBaseManager<Stack>.BuildTable("stacks",
        [
            "Id INTEGER PRIMARY KEY",
            "Name TEXT"
        ]);

        await DataBaseManager<FlashcardDTO>.BuildTable("flash_cards",
        [
            "Stacks_Id INTEGER NOT NULL",
            "FOREIGN KEY (Stacks_Id) REFERENCES stacks (Id)",
            "Id INTEGER PRIMARY KEY",
            "Front TEXT",
            "Back TEXT"
        ]);

        await DataBaseManager<Stack>.InsertLog("stacks", 
        [
            "1", 
            "'ababa'"
        ]);

        List<Stack> results = await DataBaseManager<Stack>.GetAllLogs("stacks");

        AnsiConsole.MarkupLine("Id\tName");
        foreach (Stack result in results)
        {
            AnsiConsole.WriteLine($"{result.Id}\t{result.Name}");
        }
    }
}
