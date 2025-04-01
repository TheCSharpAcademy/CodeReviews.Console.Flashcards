using System.Threading.Tasks;
using Spectre.Console;

class ManageStacksMenuController
{
    public static async Task Start()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            List<Stack> dataSet = await DataBaseManager<Stack>.GetAllLogs();
            DisplayData.Table(dataSet);

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

            if (!exit)
            {
                AnsiConsole.Markup("[bold green]Press Enter to continue. [/]");
                Console.Read();
            }
        }
            
    }

    static async Task CreateStack()
    {
        string name = GetInput.StackName();

        await DataBaseManager<Stack>.InsertLog( 
        [
            "'" + name + "'"
        ]);
    }
    static async Task RenameStack()
    {
        AnsiConsole.MarkupLine("[bold gray]Renameing stack[/]");
        List<Stack> dataSet = await DataBaseManager<Stack>.GetAllLogs();
        Stack userStack = DisplayData.Selection(dataSet);

        string newName = GetInput.StackName();

        await DataBaseManager<Stack>.UpdateLog(userStack.Id, [
            "Name = '" + newName + "'"
        ]);
    }
    static async Task DeleteStack()
    {
        AnsiConsole.MarkupLine("[bold gray]Deleting stack[/]");
        List<Stack> dataSet = await DataBaseManager<Stack>.GetAllLogs();
        Stack userStack = DisplayData.Selection(dataSet);

        await DataBaseManager<Stack>.DeleteLog(userStack.Id);
    }
}