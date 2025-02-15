using Flashcards.Dreamfxx.Data;
using Flashcards.Dreamfxx.Models;
using Flashcards.Dreamfxx.UserInput;
using Spectre.Console;

namespace Flashcards.Dreamfxx.Services;

public class StacksService(DatabaseManager databaseManager)
{
    private readonly DatabaseManager _databaseManager = databaseManager;

    public Stack ShowAllStacks()
    {
        var stacks = _databaseManager.GetStacks();
        stacks.Add(new Stack { Id = 0, Name = "Quit" });

        if (stacks == null)
        {
            AnsiConsole.WriteLine("No stacks found. Press any key to continue.");
            Console.ReadKey();

            return null;
        }

        var menuSelection = AnsiConsole.Prompt(
            new SelectionPrompt<Stack>()
            .Title("Select an option")
            .PageSize(10)
            .AddChoices(stacks)
            .UseConverter(option => option.Name));

        return menuSelection;
    }

    public void DisplayStacks()
    {
        var stacks = _databaseManager.GetStacks();

        if (stacks == null)
        {
            AnsiConsole.MarkupLine("Stacks not found. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        foreach (var stack in stacks)
        {
            AnsiConsole.WriteLine($"ID: {stack.Id}, Description: {stack.Name}, Description: {stack.Description}");
        }
    }

    public void GetSelectedStack()
    {
        var selectedStackId = ShowAllStacks().Id;
        var selectedStack = _databaseManager.GetStackDtos(selectedStackId);

        AnsiConsole.WriteLine($"Selected stack: {selectedStack.StackName}");

        foreach (var card in selectedStack.FlashcardsDto)
        {
            AnsiConsole.WriteLine($"Question: {card.Question}, Answer: {card.Answer}");
        }
        AnsiConsole.MarkupLine("[green]Press any key to continue[/]");
        Console.ReadKey();
    }

    public void CreateStack()
    {
        Console.Clear();

        while (true)
        {
            AnsiConsole.MarkupLine("[underline]Existing stacks[/]: ");
            DisplayStacks();

            string name = GetUserInput.GetUserString("Enter name of the new stack of flashcards.\n[grey]info: enter 'q' to cancel[/]\n");

            if (name == "q")
            {
                AnsiConsole.WriteLine("New stack was not created. Press any key to continue");
                Console.ReadKey();
                break;
            }

            if (CheckIfStackExists(name))
            {
                break;
            }

            string description = GetUserInput.GetUserString("Add description and possible notes to this stack: ");

            _databaseManager.CreateStack(name, description);

            AnsiConsole.MarkupLine("Stack created successfully!");
            AnsiConsole.MarkupLine("Do you want to add another stack?(y/n)");

            string? choice = Console.ReadLine();

            if (choice.ToLower() == "n")
            {
                break;
            }
        }
    }

    public void EditStack()
    {
        AnsiConsole.Clear();

        while (true)
        {
            var stack = ShowAllStacks();

            if (stack == null)
            {
                AnsiConsole.MarkupLine("No stacks found. Press any key to continue.");
                Console.ReadKey();
                break;
            }

            else if (stack.Name == "Cancel")
            {
                AnsiConsole.MarkupLine("Edit was canceled.");
                Console.ReadKey();
                break;
            }

            AnsiConsole.MarkupLine($"Current stack name: {stack.Name} \n");
            string name = GetUserInput.GetUserString("Enter the new name of the stack(Leave blank if you don't want to edit):");

            if (CheckIfStackExists(name))
            {
                break;
            }

            if (name == null)
            {
                name = stack.Name;
            }

            AnsiConsole.MarkupLine($"Current description: {stack.Description} \n");
            string description = GetUserInput.GetUserString("Enter the new description of the stack(Leave blank if you don't want to edit):");

            if (description == null)
            {
                description = stack.Description;
            }

            AnsiConsole.MarkupLine("Old stack: ");
            AnsiConsole.MarkupLine($"Stack name: {stack.Name} - Stack description: {stack.Description}\n");

            AnsiConsole.MarkupLine("New stack: ");
            AnsiConsole.MarkupLine($"Stack name: {name} - Stack description: {description}\n");

            if (!ConfirmationPrompt("update"))
            {
                break;
            }

            _databaseManager.UpdateStack(name, description, stack.Id);

            AnsiConsole.MarkupLine("[green]Stack updated successfully![/]");
            AnsiConsole.MarkupLine("[yellow]Do you want to edit another stack? - y/n[/]");

            if (Console.ReadLine().ToLower() == "n")
            {
                break;
            }
        }
    }

    public void DeleteStack()
    {
        AnsiConsole.Clear();

        while (true)
        {
            AnsiConsole.MarkupLine("Select stack that you want to delete.");
            int stackId = ShowAllStacks().Id;

            if (!ConfirmationPrompt("delete"))
            {
                break;
            }
            _databaseManager.DeleteStack(stackId);

            AnsiConsole.MarkupLine("[green]Stack deleted successfully![/]");
            AnsiConsole.MarkupLine("[yellow]Do you want to delete another stack? - y/n[/]");

            if (Console.ReadLine().ToLower() == "n")
            {
                break;
            }
        }
    }

    public bool ConfirmationPrompt(string operation)
    {
        AnsiConsole.Clear();

        var menuRoute = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title($"Do you really want to {operation} this stack?")
            .PageSize(5)
            .AddChoices(["Yes", "No"])
            .UseConverter(option => option));

        if (menuRoute == "Yes")
        {
            return true;
        }
        return false;
    }

    public bool CheckIfStackExists(string stackName)
    {
        var stacks = _databaseManager.GetStacks();
        foreach (var stack in stacks)
        {
            if (stack.Name.ToLower() == stackName.ToLower())
            {
                AnsiConsole.Markup("[red]This name already belong to existing stack, choose different name.[/]\nPress any key to continue.");
                Console.ReadKey();
                return true;
            }
        }
        return false;
    }
}