using FlashardsUI.Helpers;
using FlashcardsLibrary.Models;
using FlashcardsLibrary.Repositories;
using Spectre.Console;

namespace FlashardsUI;
internal class StacksController
{
    private readonly IStacksRepository _stacksRepository;
    public StacksController(IStacksRepository stacksRepository)
    {
        _stacksRepository = stacksRepository;
    }
    internal async Task GetAll()
    {
        IEnumerable<Stack> stacks = await _stacksRepository.GetAllAsync();

        List<StackDTO> stackDTOs = new();

        foreach (var s in stacks)
        {
            stackDTOs.Add(s.ToStackDTO());
        }

        TableVisualization.ShowStacksTable(stackDTOs);

        AnsiConsole.Write("Press any key to continue... ");
        Console.ReadKey();
    }

    internal async Task Post()
    {
        var name = UserInput.StringPrompt("Please enter the stack name (or press 0 to cancel):");

        while (await _stacksRepository.StackNameExistsAsync(name.Trim()))
        {
            Console.Clear();
            name = UserInput.StringPrompt($"Stack with the name [red]{name}[/] already exists! Enter stack name (or press 0 to cancel):");
        }

        if (name.Trim() == "0")
        {
            return;
        }

        await _stacksRepository.AddAsync(new Stack
        {
            Name = name.Trim()
        });

        AnsiConsole.Markup($"\nNew stack [green]{name}[/] was succesfully added! Press any key to continue...");
        Console.ReadKey();
    }

    internal async Task Delete()
    {
        var stack = await Get("Select a stack to delete:");

        if (stack.Id == 0)
        {
            return;
        }

        if (!AnsiConsole.Confirm($"Are you sure you want to delete [green]{stack.Name}[/] stack ? \nThis will delete all associated flashcards and study sessions!"))
        {
            return;
        }

        await _stacksRepository.DeleteAsync(stack);

        AnsiConsole.Markup($"\nStack [green]{stack.Name}[/] was succesfully deleted! Press any key to continue...");
        Console.ReadKey();
    }

    internal async Task Update()
    {
        var stack = await Get("Select a stack to update:");

        if (stack.Id == 0)
        {
            return;
        }

        var name = UserInput.StringPrompt($"Enter a new name for the stack [blue]{stack.Name}[/] (or press 0 to cancel):");

        while (await _stacksRepository.StackNameExistsAsync(name.Trim()))
        {
            Console.Clear();
            name = UserInput.StringPrompt($"Stack with the name [red]{name}[/] already exists! Enter stack name (or press 0 to cancel):");
        }

        if (name.Trim() == "0")
        {
            return;
        }

        if (!AnsiConsole.Confirm($"Are you sure you want to rename stack [blue]{stack.Name}[/] to [green]{name}[/]?"))
        {
            return;
        }

        stack.Name = name.Trim();
        await _stacksRepository.UpdateAsync(stack);

        AnsiConsole.Markup($"\nStack was succesfully updated to [green]{name}[/]! Press any key to continue...");
        Console.ReadKey();
    }

    internal async Task<Stack> Get(string prompt)
    {
        IEnumerable<Stack> stacks = await _stacksRepository.GetAllAsync();

        return AnsiConsole.Prompt(
            new SelectionPrompt<Stack>()
            .Title(prompt)
            .AddChoices(stacks)
            .AddChoices(new Stack { Id = 0, Name = "Cancel and return to menu" })
            .UseConverter(stack => stack.Name!)
            );
    }
}
