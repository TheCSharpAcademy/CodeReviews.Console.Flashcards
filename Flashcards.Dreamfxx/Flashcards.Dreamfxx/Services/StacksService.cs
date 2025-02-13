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
            AnsiConsole.WriteLine($"ID: {stack.Id}, Name: {stack.Name}, Description: {stack.Description}");
        }
    }

    public void GetSelectedStack()
    {
        var selectedStackId = ShowAllStacks().Id;
        var selectedStack = _databaseManager.GetCardStackDTOs(selectedStackId);

        AnsiConsole.WriteLine($"Selected stack: {selectedStack.CardStackName}");

        foreach (var card in selectedStack.Cards)
        {
            AnsiConsole.WriteLine($"Question: {card.Question}, Answer: {card.Answer}");
        }
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

            Console.WriteLine("Stack created successfully!");
            Console.WriteLine("Do you want to add another stack?(y/n)");

            string? choice = Console.ReadLine();

            if (choice.ToLower() == "n")
            {
                break;
            }
        }
    }

    public void EditStack()
    {
        Console.Clear();
        while (true)
        {
            var stack = ShowAllStacks();
            if (stack == null)
            {
                Console.ReadKey();
                break;
            }
            else if (stack.Name == "Cancel")
            {
                Console.WriteLine("Operation canceled!");
                Console.ReadKey();
                break;
            }

            Console.WriteLine($"Current stack name: {stack.Name} \n");
            string name = GetUserInput.GetUserString("Enter the new name of the stack(Leave blank if you don't want to edit):");

            if (CheckIfStackExists(name))
            {
                break;
            }

            if (name == null)
            {
                name = stack.Name;
            }

            Console.WriteLine($"Current description: {stack.Description} \n");
            string description = GetUserInput.GetUserString("Enter the new description of the stack(Leave blank if you don't want to edit):");

            if (description == null)
            {
                description = stack.Description;
            }

            Console.WriteLine("Old stack: ");
            Console.WriteLine($"Stack name: {stack.Name} - Stack description: {stack.Description}\n");

            Console.WriteLine("New stack: ");
            Console.WriteLine($"Stack name: {name} - Stack description: {description}\n");

            if (!ConfirmationPrompt("update"))
            {
                break;
            }

            _databaseManager.UpdateStack(name, description, stack.Id);

            Console.WriteLine("Stack updated successfully!");
            Console.WriteLine("Do you want to edit another stack?(y/n)");

            if (Console.ReadLine().ToLower() == "n")
            {
                break;
            }
        }
    }

    public void DeleteStack()
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("Select the stack you want to delete:");
            int stackId = ShowAllStacks().Id;

            if (!ConfirmationPrompt("delete"))
            {
                break;
            }
            _databaseManager.DeleteStack(stackId);

            Console.WriteLine("Stack deleted successfully!");
            Console.WriteLine("Do you want to delete another stack?(y/n)");

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
            .Title($"Are you sure you want to do {operation} on this stack?")
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
                Console.WriteLine("This stack name is already taken.");
                Console.ReadKey();
                return true;
            }
        }
        return false;
    }
}