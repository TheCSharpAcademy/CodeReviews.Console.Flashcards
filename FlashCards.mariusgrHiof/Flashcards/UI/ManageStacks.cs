using Flashcards.Controllers;
using Flashcards.Models;
using Flashcards.Utils;
using Spectre.Console;

namespace Flashcards.UI;

public class ManageStacks
{
    private readonly StacksController _stacksController;

    public ManageStacks(StacksController stacksController)
    {
        _stacksController = stacksController;
    }

    public async Task ShowMenu()
    {


        bool exitManageStacks = false;

        while (!exitManageStacks)
        {
            AnsiConsole.Clear();

            ShowMenuItems();

            Console.Write("Enter a number: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    exitManageStacks = true;
                    break;
                case "2":
                    await ShowStacks();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    break;
                case "3":
                    await HandeAddNewStack();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    break;
                case "4":
                    await InteractWithStack();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Invalid number.Try again");
                    break;
            }
        }
    }

    private async Task InteractWithStack()
    {
        InteractWithStack interactiveStacks = new InteractWithStack(_stacksController);
        await interactiveStacks.ShowMenu();
    }

    private async Task<Stack> HandeAddNewStack()
    {
        await ShowStacks();

        string stackName = GetStringUserInput("Enter a stack name(0 to exit): ");

        if (stackName == "0")
        {

            return null;
        }

        var existingStack = await _stacksController.GetStackByNameAsync(stackName);

        while (existingStack != null)
        {
            Console.WriteLine("Stack already exists.Try again with another stack name");
            stackName = GetStringUserInput("Enter a stack name(0 to exit): ");

            if (stackName == "0")
            {

                return null;
            }

            existingStack = await _stacksController.GetStackByNameAsync(stackName);
        }

        var newStack = await _stacksController.AddStackAsync(new Stack
        {
            Name = stackName,
        });



        if (newStack != null)
        {
            Console.WriteLine("Record has been added!");
        }
        else
        {
            Console.WriteLine("Fail to add record!");
        }

        return newStack;
    }

    private async Task ShowStacks()
    {
        var stacks = await _stacksController.GetAllStacksAsync();

        Table table = new Table();
        table.AddColumns("Stack name");

        foreach (var stack in stacks)
        {
            table.AddRow(stack.Name);
        }

        AnsiConsole.Write(table);
    }
    private void ShowMenuItems()
    {
        Console.WriteLine("Manage Stacks");
        Console.WriteLine("1. Go back to Main Menu");
        Console.WriteLine("2. Show stacks");
        Console.WriteLine("3. Add new stack");
        Console.WriteLine("4. Interact with current stack");
    }

    private string GetStringUserInput(string message)
    {
        Console.Write(message);
        string input = Console.ReadLine();
        while (!Validate.IsValidString(input))
        {
            Console.WriteLine("Invalid input.Try again.");
            Console.Write(message);
            input = Console.ReadLine();
        }

        return input;
    }
}