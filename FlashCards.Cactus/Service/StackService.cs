using FlashCards.Cactus.Dao;
using FlashCards.Cactus.DataModel;
using FlashCards.Cactus.Helper;
using Spectre.Console;

namespace FlashCards.Cactus.Service;
public class StackService
{
    public StackService(StackDao stackDao)
    {
        StackDao = stackDao;
        Stacks = StackDao.FindAll();
    }

    public StackDao StackDao { get; set; }

    public List<Stack> Stacks { get; set; }

    public void ShowStacks()
    {
        List<List<string>> rows = new List<List<string>>();
        Stacks.ForEach(stack => rows.Add(new List<string>() { stack.Name }));
        ServiceHelper.ShowDataRecords(Constants.STACK, Constants.STACKS, rows);
    }

    public void AddStack()
    {
        string name = AnsiConsole.Ask<string>("Please input the [green]name[/] of a new stack. Type 'q' to quit. (No more than 50 characters.)");
        if (name.Equals(Constants.QUIT)) return;
        do
        {
            if (name.Length <= 50) break;
            name = AnsiConsole.Ask<string>("The name cannot exceed [red]50[/] characters. Please input a valid name. Type 'q' to quit.");
            if (name.Equals(Constants.QUIT)) return;
        } while (true);

        int id = Stacks[Stacks.Count - 1].Id + 1;
        Stack stack = new Stack(id++, name);

        int res = StackDao.Insert(stack);
        if (res == -1)
        {
            AnsiConsole.MarkupLine($"[red]Failed to add {name} Stack.[/]");
        }
        else
        {
            Stacks.Add(stack);
            AnsiConsole.MarkupLine($"Successfully added [green]{name}[/] Stack.");
        }
    }

    public void DeleteStack()
    {
        ShowStacks();

        if (Stacks.Count == 0) return;

        string name = AnsiConsole.Ask<string>("Please input the [green]name[/] of the Stack will be deleted. Type 'q' to quit.");
        if (name.Equals(Constants.QUIT)) return;

        string[] stackNames = Stacks.Select(s => s.Name).ToArray();
        while (!stackNames.Contains(name))
        {
            name = AnsiConsole.Ask<string>($"[red]{name} dose not exist[/]. Please input a valid name. Type 'q' to quit.");
            if (name.Equals(Constants.QUIT)) return;
        }

        int res = StackDao.DeleteByName(name);
        if (res == -1)
        {
            AnsiConsole.MarkupLine($"[red]Failed to delete {name} Stack.[/]");
        }
        else
        {
            Stacks = Stacks.Where(s => !s.Name.Equals(name)).ToList();
            AnsiConsole.MarkupLine($"Successfully deleted [green]{name}[/] Stack.");
        }
    }
}

