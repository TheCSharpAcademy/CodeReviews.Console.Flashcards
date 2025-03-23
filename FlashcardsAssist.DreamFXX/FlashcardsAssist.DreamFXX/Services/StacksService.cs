using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsAssist.DreamFXX.Models;
using FlashcardsAssist.DreamFXX.Data;
using Spectre.Console;

namespace FlashcardsAssist.DreamFXX.Services;
public class StacksService
{
    private readonly DatabaseService _dbService;

    public StacksService(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    public async Task CreateStackAsync()
    {
        var stackName = AnsiConsole.Ask<string>("[yellow]Enter stack name:[/]");
        
        try
        {
            await _dbService.CreateStackAsync(stackName);
            AnsiConsole.MarkupLine($"[green]Stack '{stackName}' created successfully![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error creating stack: {ex.Message}[/]");
        }
    }

    public async Task<Stack> SelectStackAsync()
    {
        var stacks = await _dbService.GetAllStacksAsync();
        
        if (!stacks.Any())
        {
            AnsiConsole.MarkupLine("[red]No stacks found. Please create a stack first.[/]");
            return null;
        }

        var stackName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Select a stack:[/]")
                .PageSize(10)
                .AddChoices(stacks.Select(s => s.Name)));

        return await _dbService.GetStackByNameAsync(stackName);
    }

    public async Task DeleteStackAsync()
    {
        var stacks = await _dbService.GetAllStacksAsync();
        
        if (!stacks.Any())
        {
            AnsiConsole.MarkupLine("[red]No stacks found. Nothing to delete.[/]");
            return;
        }

        var stackName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Select a stack to delete:[/]")
                .PageSize(10)
                .AddChoices(stacks.Select(s => s.Name)));

        if (AnsiConsole.Confirm($"[red]Are you sure you want to delete stack '{stackName}'? This will also delete all flashcards and study sessions associated with it.[/]"))
        {
            await _dbService.DeleteStackAsync(stackName);
            AnsiConsole.MarkupLine($"[green]Stack '{stackName}' deleted successfully![/]");
        }
    }

    public async Task ViewAllStacksAsync()
    {
        var stacks = await _dbService.GetAllStacksAsync();
        
        if (!stacks.Any())
        {
            AnsiConsole.MarkupLine("[red]No stacks found.[/]");
            return;
        }

        var table = new Table()
            .Title("[yellow]Available Stacks[/]")
            .AddColumn(new TableColumn("ID").Centered())
            .AddColumn(new TableColumn("Name").LeftAligned());

        foreach (var stack in stacks)
        {
            table.AddRow(stack.Id.ToString(), stack.Name);
        }

        AnsiConsole.Write(table);
    }
}
