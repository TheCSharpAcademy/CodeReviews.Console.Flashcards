using Flashcards.Data;
using Flashcards.Helpers;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.Services;

public class StackService
{
    private const int MaxStackNameLength = 100;
    private readonly AppDbContext _dbContext;

    public StackService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreateStack(string stackName)
    {
        // Validate stack name
        if (!ValidationHelper.ValidateString(stackName, "Stack name", MaxStackNameLength)) return;

        // Create and save stack
        var stack = new Stack { Name = stackName.Trim() };
        _dbContext.Stacks.Add(stack);
        _dbContext.SaveChanges();
        AnsiConsole.MarkupLine("[green]Stack created successfully![/]");
    }

    public List<Stack> GetAllStacks()
    {
        var stacks = _dbContext.Stacks.ToList();

        if (stacks.Count == 0) AnsiConsole.MarkupLine("[yellow]No stacks found.[/]");

        return stacks;
    }

    public bool UpdateStack(int stackId, string newName)
    {
        // Validate stack ID and name
        if (!ValidationHelper.ValidateId(stackId, "stack") ||
            !ValidationHelper.ValidateString(newName, "Stack name", MaxStackNameLength))
            return false;

        // Find and update stack
        var stack = _dbContext.Stacks.Find(stackId);
        if (stack != null)
        {
            stack.Name = newName.Trim();
            _dbContext.SaveChanges();
            AnsiConsole.MarkupLine("[green]Stack updated successfully![/]");
            return true;
        }

        AnsiConsole.MarkupLine("[red]Error: Stack not found.[/]");
        return false;
    }

    public bool DeleteStack(int stackId)
    {
        // Validate stack ID
        if (!ValidationHelper.ValidateId(stackId, "stack")) return false;

        // Find and delete stack
        var stack = _dbContext.Stacks.Find(stackId);
        if (stack != null)
        {
            _dbContext.Stacks.Remove(stack);
            _dbContext.SaveChanges();
            AnsiConsole.MarkupLine("[green]Stack deleted successfully![/]");
            return true;
        }

        AnsiConsole.MarkupLine("[red]Error: Stack not found.[/]");
        return false;
    }
}