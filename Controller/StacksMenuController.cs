using System.Threading.Tasks;
using Spectre.Console;

class StacksMenuController : MenuController
{
    static List<Stack> stacks = [];
    static StacksDatabaseManager stacksDatabaseManager = new();
    static FlashcardsDatabaseManager flashcardsDatabaseManager = new();
    static StudySessionDatabaseManager studySessionDatabaseManager = new();

    // Get all stacks and display all stacks to user
    protected override async Task MainAsync()
    {
        stacks = await stacksDatabaseManager.GetLogs();
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

        await stacksDatabaseManager.InsertLog(new Stack(default, name));
    }
    static async Task RenameStack()
    {
        AnsiConsole.MarkupLine("[bold gray]Renameing stack[/]");

        // User selects stack
        Stack userStack = GetInput.Selection(stacks);

        // User inputs new name
        string newName = GetInput.StackName(stacks);

        // Update stack
        await stacksDatabaseManager.UpdateLog(new Stack(userStack.Id, newName));
    }
    static async Task DeleteStack()
    {
        AnsiConsole.MarkupLine("[bold gray]Deleting stack[/]");

        // User selects stack
        Stack userStack = GetInput.Selection(stacks);

        // Deleting flash cards
        List<Flashcard> flashcards = await flashcardsDatabaseManager.GetLogs(userStack);
        foreach (var card in flashcards)
        {
            await flashcardsDatabaseManager.DeleteLog(card.Id);
        }

        // Deleting study sessions
        List<StudySession> studySessions = await studySessionDatabaseManager.GetLogs(userStack);
        foreach (var session in studySessions)
        {
            await studySessionDatabaseManager.DeleteLog(session.Id);
        }

        // Deleting now empty stack
        await stacksDatabaseManager.DeleteLog(userStack.Id);
    }
}