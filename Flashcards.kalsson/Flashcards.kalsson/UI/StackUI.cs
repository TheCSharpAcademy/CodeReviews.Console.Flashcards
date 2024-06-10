using Flashcards.kalsson.Models;
using Flashcards.kalsson.Services;
using Spectre.Console;

namespace Flashcards.kalsson.UI;

public class StackUI
{
    private readonly StackService _stackService;

    public StackUI(StackService stackService)
    {
        _stackService = stackService;
    }

    public void ShowAllStacks()
    {
        var stacks = _stackService.GetAllStacks();
        var table = new Table();

        table.AddColumn("ID");
        table.AddColumn("Name");

        foreach (var stack in stacks)
        {
            table.AddRow(stack.Id.ToString(), stack.Name);
        }

        AnsiConsole.Write(table);
    }

    public void AddStack()
    {
        var name = AnsiConsole.Ask<string>("Enter stack name:");
        var stack = new Stack { Name = name };
        _stackService.AddStack(stack);
    }

    public void DeleteStack()
    {
        var id = AnsiConsole.Ask<int>("Enter stack ID to delete:");
        _stackService.DeleteStack(id);
    }
}