using System.Data.SqlClient;
using Flashcards.Mateusz_Platek.Models;
using Flashcards.Mateusz_Platek.Repositories;
using Spectre.Console;

namespace Flashcards.Mateusz_Platek.Controllers;

public static class StackController
{
    private static StacksRepository stacksRepository = new StacksRepository();
    
    public static Stack? SelectStack()
    {
        List<Stack> stacks = stacksRepository.GetStacks();
        if (stacks.Count == 0)
        {
            return null;
        }

        return AnsiConsole.Prompt(
            new SelectionPrompt<Stack>()
                .Title("[bold red]Select stack:[/]")
                .AddChoices(stacks)
                .UseConverter(stack => $"{stack.name}")
        );
    }
    
    public static void DisplayStacks()
    {
        Table table = new Table()
            .Title("[bold red]Stacks[/]")
            .HideHeaders()
            .AddColumn("stack name");
        
        List<Stack> stacks = stacksRepository.GetStacks();
        foreach (Stack stack in stacks)
        {
            table.AddRow($"[bold purple]{stack.name}[/]");
        }
        
        AnsiConsole.Write(table);
    }
    
    public static void AddStack()
    {
        string stackName = AnsiConsole.Prompt(
            new TextPrompt<string>("[bold purple]Insert stack name:[/]")
        );

        try
        {
            stacksRepository.AddStack(stackName);
        }
        catch (SqlException)
        {
            AnsiConsole.Write(
                new Markup("[bold red]Stack already exists[/]\n")
            );
            return;
        }
        
        AnsiConsole.Write(
            new Markup("[bold green]New stack added[/]\n")
        );
    }
    
    public static void UpdateStack()
    {
        Stack? stack = SelectStack();
        if (stack == null)
        {
            AnsiConsole.Write(
                new Markup("[bold red]List of stacks is empty[/]")
            );
            return;
        }
        
        string newName = AnsiConsole.Prompt(
            new TextPrompt<string>("[bold purple]Insert new stack name:[/]")
        );

        try
        {
            stacksRepository.UpdateStack(stack.name, newName);
        }
        catch (SqlException)
        {
            AnsiConsole.Write(
                new Markup("[bold red]Stack already exists[/]\n")
            );
            return;
        }
        
        AnsiConsole.Write(
            new Markup("[bold green]Stack updated[/]\n")
        );
    }
    
    public static void DeleteStack()
    {
        Stack? stack = SelectStack();
        if (stack == null)
        {
            AnsiConsole.Write(
                new Markup("[bold red]List of stacks is empty[/]")
            );
            return;
        }

        stacksRepository.DeleteStack(stack.name);
        
        AnsiConsole.Write(
            new Markup("[bold green]Stack removed[/]\n")
        );
    }
}