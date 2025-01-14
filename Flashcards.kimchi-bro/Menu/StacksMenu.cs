using Spectre.Console;

internal class StacksMenu
{
    private static readonly Dictionary<string, Action> _stacksMenuActions = new()
    {
        { DisplayInfoHelpers.Back, Console.Clear },
        { "Show all stacks", StackRead.ShowAllStacks },
        { "Create stack", StackCreate.Create },
        { "Update stack", StackUpdate.Update },
        { "Delete stack", StackDelete.Delete }
    };

    internal static void ShowStacksMenu()
    {
        Console.Clear();
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an action with stack: ")
            .PageSize(10)
            .AddChoices(_stacksMenuActions.Keys));

        _stacksMenuActions[choice]();
    }
}
