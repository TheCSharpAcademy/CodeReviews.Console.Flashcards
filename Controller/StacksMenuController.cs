using System.Threading.Tasks;
using Spectre.Console;

class StacksMenuController : MenuController
{
    // Display all stacks to user
    protected override async Task MainAsync()
    {
        List<Stack> dataSet = await DataBaseManager<Stack>.GetLogs();
        DisplayData.Table(dataSet);
    }

    protected override async Task<bool> HandleMenuAsync()
    {
        Enums.ManageStacksMenuOptions userInput = DisplayMenu.StacksMenu();
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
                return true;
        }
        return false;
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

        // User selects stack
        List<Stack> dataSet = await DataBaseManager<Stack>.GetLogs();
        Stack userStack = GetInput.Selection(dataSet);

        // User inputs new name
        string newName = GetInput.StackName();

        // Update stack
        await DataBaseManager<Stack>.UpdateLog(
            "Id = " + userStack.Id.ToString(), 
            [
                "Name = '" + newName + "'"
            ]);
    }
    static async Task DeleteStack()
    {
        AnsiConsole.MarkupLine("[bold gray]Deleting stack[/]");

        // User selects stack
        List<Stack> dataSet = await DataBaseManager<Stack>.GetLogs();
        Stack userStack = GetInput.Selection(dataSet);

        // Deleting flash cards
        List<Flashcard> flashcards = await DataBaseManager<Flashcard>.GetLogs();
        foreach (var card in flashcards)
        {
            await DataBaseManager<Flashcard>.DeleteLog(card.Id);
        }

        // Deleting now empty stack
        await DataBaseManager<Stack>.DeleteLog(userStack.Id);
    }
}