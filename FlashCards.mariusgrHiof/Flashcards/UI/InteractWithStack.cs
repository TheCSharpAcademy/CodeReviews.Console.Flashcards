using Flashcards.Controllers;
using Flashcards.Models;
using Flashcards.Utils;
using Spectre.Console;

namespace Flashcards.UI;

public class InteractWithStack
{
    private readonly StacksController _controller;

    public InteractWithStack(StacksController controller)
    {
        _controller = controller;
    }
    public async Task ShowMenu()
    {

        AnsiConsole.WriteLine("Manage Stacks");
        bool exitManageStacks = false;

        Stack stack = null;

        stack = await GetStackFromUser();

        if (stack == null)
        {
            return;
        }

        while (!exitManageStacks)
        {
            AnsiConsole.Clear();

            Console.WriteLine($"Current stack: {stack.Name}");

            ShowMenuItems();
            Console.Write("Enter a number: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    exitManageStacks = true;
                    break;
                case "2":
                    await HandleUpdateStackName(stack.Id);
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    break;
                case "3":
                    await HandleDeleteStack(stack.Id);
                    exitManageStacks = true;
                    break;
                default:
                    Console.WriteLine("Invalid number.Try again");
                    break;
            }
        }
    }

    private async Task<Stack> HandleDeleteStack(int stackId)
    {
        var stack = await _controller.GetStackByIdAsync(stackId);

        if (stack == null)
        {
            Console.WriteLine("No record found");
            return null;
        }

        var result = await _controller.DeleteStackByIdAsync(stackId);



        if (result != null)
        {
            Console.WriteLine("Record has been deleted!");
        }
        else
        {
            Console.WriteLine("Fail to delete record!");
        }

        return result;
    }

    private async Task HandleUpdateStackName(int stackId)
    {
        Console.Write("Enter a stack name: ");
        string stackName = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(stackName))
        {
            Console.WriteLine("Invalid input.Try again");
            Console.Write("Enter a stack name: ");

            stackName = Console.ReadLine();
        }

        var stackToBeUpdated = await _controller.UpdateStackByIdAsync(stackId, new Stack
        {
            Name = stackName,
            Id = stackId
        });



        if (stackToBeUpdated == null)
        {
            Console.WriteLine("Fail to update");
        }
        else
        {
            Console.WriteLine("Stack ha been updated!");
        }

    }

    public void ShowMenuItems()
    {


        Console.WriteLine("1. Go back to Manage Stacks");
        Console.WriteLine("2. Update stack name");
        Console.WriteLine("3. Delete stack");
    }

    public async Task ShowStacks()
    {
        var stacks = await _controller.GetAllStacksAsync();
        stacks = stacks.OrderBy(s => s.Id).ToList();

        Table table = new Table();

        table.AddColumns("Name");

        foreach (var stack in stacks)
        {
            table.AddRow(stack.Name);
        }

        AnsiConsole.Write(table);
    }

    private async Task<Stack> GetStackFromUser()
    {
        Stack stack = null;

        await ShowStacks();

        String stackInput = GetStringUserInput("Enter a stack to interact with(0 to exit): ");

        if (stackInput == "0")
        {
            return null;
        }

        stack = await _controller.GetStackByNameAsync(stackInput);

        while (stack == null)
        {
            Console.WriteLine("No record foudn.Try again.");
            stackInput = GetStringUserInput("Enter a stack to interact with(0 to exit): ");

            if (stackInput == "0")
            {
                return null;
            }

            stack = await _controller.GetStackByNameAsync(stackInput);
        }


        return stack;
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