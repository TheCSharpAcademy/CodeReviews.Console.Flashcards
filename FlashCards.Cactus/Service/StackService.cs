using FlashCards.Cactus.DataModel;
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
        Console.WriteLine("Please input the name of a new stack. Type 'q' to quit. (No more than 50 characters.)");
        string name = Console.ReadLine();
        if (name.Equals(QUIT)) return;
        do
        {
            if (name.Length <= 50) break;
            Console.WriteLine("The name cannot exceed 50 characters. Please input a valid name. Type 'q' to quit.");
            name = Console.ReadLine();
            if (name.Equals(QUIT)) return;
        } while (true);

        int id = Stacks[Stacks.Count - 1].Id + 1;
        Stack stack = new Stack(id++, name);
        Stacks.Add(stack);

        Console.WriteLine($"Successfully added {name} Stack.");
    }

    public void DeleteStack()
    {
        ShowStacks();
        Console.WriteLine("Please input the name of the Stack will be deleted. Type 'q' to quit.");
        string name = Console.ReadLine();
        if (name.Equals(QUIT)) return;

        string[] stackNames = Stacks.Select(s => s.Name).ToArray();
        while (!stackNames.Contains(name))
        {
            Console.WriteLine($"{name} dose not exist. Please input a valid name.");
            name = Console.ReadLine();
        }
        Stacks = Stacks.Where(s => !s.Name.Equals(name)).ToList();

        Console.WriteLine($"Successfully deleted {name} Stack.");
    }
}

