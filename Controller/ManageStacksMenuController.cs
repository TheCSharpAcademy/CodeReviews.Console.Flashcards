using System.Threading.Tasks;
using Spectre.Console;

class ManageStacksMenuController
{
    const string tableName = "stacks";
    public static async Task Start()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Enums.ManageStacksMenuOptions userInput = DisplayMenu.ManageStacksMenu();

            switch (userInput)
            {
                case Enums.ManageStacksMenuOptions.CREATESTACK:
                    await CreateStack();
                    break;
                case Enums.ManageStacksMenuOptions.RENAMESTACK:
                    await RenameStack();
                    break;
                case Enums.ManageStacksMenuOptions.DELETESTACK:
                    await DeleteStack();
                    break;
                case Enums.ManageStacksMenuOptions.BACK:
                    exit = true;
                    break;
            }

            AnsiConsole.Markup("[bold green]Press Enter to continue. [/]");
            Console.Read();
        }
            
    }

    static async Task CreateStack()
    {
        await DataBaseManager.InsertLog(tableName, 
        [
            "1",
            "'GOTY'"
        ]);
    }
    static async Task RenameStack()
    {
        List<Stack> dataSet = await DataBaseManager.GetAllLogs<Stack>(tableName);

        DisplayOther.DisplayTable(dataSet, ["Id", "Name"]);
    }
    static async Task DeleteStack(){}
}