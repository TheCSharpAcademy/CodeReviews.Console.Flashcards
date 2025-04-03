using System.Threading.Tasks;
using Spectre.Console;

class StacksMenuController : MenuController
{

    static List<Stack> stacks = [];

    // Get all stacks and display all stacks to user
    protected override async Task MainAsync()
    {
        stacks = await DataBaseManager<Stack>.GetLogs();
        DisplayData.Table(stacks);
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
        string name = GetInput.StackName(stacks);

        await DataBaseManager<Stack>.InsertLog( 
        [
            $" '{name}' "
        ]);
    }
    static async Task RenameStack()
    {
        AnsiConsole.MarkupLine("[bold gray]Renameing stack[/]");

        // User selects stack
        Stack userStack = GetInput.Selection(stacks);

        // User inputs new name
        string newName = GetInput.StackName(stacks);

        // Update stack
        await DataBaseManager<Stack>.UpdateLog(
            "Id = " + userStack.Id.ToString(), 
            [
                $"Name = '{newName}'"
            ]);
    }
    static async Task DeleteStack()
    {
        AnsiConsole.MarkupLine("[bold gray]Deleting stack[/]");

        // User selects stack
        Stack userStack = GetInput.Selection(stacks);

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