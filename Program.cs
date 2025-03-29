namespace Flashcards;


class Program
{
    static readonly int ERRORCODETABLEEXISTS = -2146232060;
    static async Task Main(string[] args)
    {
        DataBaseManager.Start();

        await DataBaseManager.BuildTable("stacks", new List<string>
        {
            "Id INTEGER PRIMARY KEY",
            "Name TEXT"
        });

        await DataBaseManager.BuildTable("flash_cards", new List<string>
        {
            "Stacks_Id INTEGER NOT NULL",
            "FOREIGN KEY (Stacks_Id) REFERENCES stacks (Id)",
            "Id INTEGER PRIMARY KEY",
            "Front TEXT",
            "Back TEXT"
        });

        await DataBaseManager.InsertLog();

        await DataBaseManager.GetAllLogs();   
    }
}
