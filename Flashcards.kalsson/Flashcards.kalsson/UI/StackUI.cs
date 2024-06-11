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
        var name = AnsiConsole.Ask<string>("Enter stack name (type 'back' to return to menu):");

        if (name.ToLower() == "back")
        {
            return;
        }
        
        var stack = new Stack { Name = name };
        try
        {
            _stackService.AddStack(stack);
            Console.Clear();
            AnsiConsole.MarkupLine("[green]Stack has been successfully added![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Failed to add the stack: {ex.Message}[/]");
        }
    }

    public void UpdateStack()
    {
        var idInput = AnsiConsole.Ask<string>("Enter stack ID to update (type 'back' to return to menu):");

        if (idInput.ToLower() == "back")
        {
            return;
        }

        var id = Convert.ToInt32(idInput);

        var name = AnsiConsole.Ask<string>("Enter new stack name (type 'back' to return to menu):");

        if (name.ToLower() == "back")
        {
            return;
        }
    
        var stack = new Stack { Id = id, Name = name };
    
        try
        {
            _stackService.UpdateStack(stack);
            Console.Clear();
            AnsiConsole.MarkupLine("[green]Stack has been successfully updated![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Failed to update the stack: {ex.Message}[/]");
        }
    }

    public void DeleteStack()
    {
        var idInput = AnsiConsole.Ask<string>("Enter stack ID to delete (type 'back' to return to menu):");

        if (idInput.ToLower() == "back")
        {
            return;
        }

        if (!int.TryParse(idInput, out int id))
        {
            AnsiConsole.MarkupLine("[red]Invalid ID input. Please enter an integer.[/]");
            return;
        }

        if (_stackService == null)
        {
            AnsiConsole.MarkupLine("[red]Error: Stack service is not initialized.[/]");
            return;
        }

        var stack = _stackService.GetStackById(id);
        if (stack == null)
        {
            AnsiConsole.MarkupLine($"[red]Stack with ID {id} does not exist.[/]");
            return;
        }

        try
        {
            _stackService.DeleteStack(id);
            Console.Clear();
            AnsiConsole.MarkupLine("[green]Stack has been successfully deleted![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Failed to delete the stack: {ex.Message}[/]");
        }
    }
}