﻿using FlashCards.Cactus.DataModel;
using Spectre.Console;

namespace FlashCards.Cactus.Service;
public class StackService
{
    private const string QUIT = "q";

    public StackService()
    {
        Stacks = new List<Stack> { new Stack(1, "Word"), new Stack(2, "Algorithm") };
    }

    public List<Stack> Stacks { get; set; }

    public void ShowStacks()
    {
        if (Stacks.Count == 0)
        {
            Console.WriteLine("No Stack exists.");
            return;
        }

        var table = new Table();
        table.Title("Stacks");
        table.Border(TableBorder.Square);
        table.Collapse();
        table.AddColumn(nameof(Stack.Id));
        table.AddColumn(new TableColumn(nameof(Stack.Name)).Centered());
        int id = 0;
        Stacks.ForEach(stack => { table.AddRow((++id).ToString(), stack.Name); });
        AnsiConsole.Write(table);
    }

    public void AddStack()
    {
        string name = AnsiConsole.Ask<string>("Please input the [green]name[/] of a new stack. Type 'q' to quit. (No more than 50 characters.)");
        if (name.Equals(QUIT)) return;
        do
        {
            if (name.Length <= 50) break;
            name = AnsiConsole.Ask<string>("The name cannot exceed [red]50[/] characters. Please input a valid name. Type 'q' to quit.");
            if (name.Equals(QUIT)) return;
        } while (true);

        int id = Stacks[Stacks.Count - 1].Id + 1;
        Stack stack = new Stack(id++, name);
        Stacks.Add(stack);

        AnsiConsole.MarkupLine($"Successfully added [green]{name}[/] Stack.");
    }

    public void DeleteStack()
    {
        ShowStacks();

        string name = AnsiConsole.Ask<string>("Please input the [green]name[/] of the Stack will be deleted. Type 'q' to quit.");
        if (name.Equals(QUIT)) return;

        string[] stackNames = Stacks.Select(s => s.Name).ToArray();
        while (!stackNames.Contains(name))
        {
            name = AnsiConsole.Ask<string>($"[red]{name} dose not exist[/]. Please input a valid name. Type 'q' to quit.");
            if (name.Equals(QUIT)) return;
        }
        Stacks = Stacks.Where(s => !s.Name.Equals(name)).ToList();

        AnsiConsole.MarkupLine($"Successfully deleted [green]{name}[/] Stack.");
    }
}
