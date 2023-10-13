using Flashcards.wkktoria.Models.Dtos;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.Validators;

namespace Flashcards.wkktoria.Controllers;

internal class StackController
{
    private readonly StackService _stackService;

    internal StackController(StackService stackService)
    {
        _stackService = stackService;
    }

    internal void ShowAll()
    {
        Console.Clear();

        var stacks = _stackService.GetAll();

        if (stacks.Any()) TableVisualisation.ShowStacksTable(stacks);
        else
            Console.WriteLine("No stacks found.");

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    internal void Create()
    {
        Console.Clear();

        var name = UserInput.GetStringInput("Enter name for new stack.");

        while (!StackValidator.CheckName(name))
        {
            Console.WriteLine($"'{name}' is invalid name.");
            name = UserInput.GetStringInput("Enter name for new stack.");
        }

        while (_stackService.CheckIfNameExists(name))
        {
            Console.WriteLine($"Stack with name '{name}' already exists.");
            name = UserInput.GetStringInput("Enter name for new stack.");
        }

        var newStack = new StackDto
        {
            Name = name
        };

        var created = _stackService.Create(newStack);

        Console.WriteLine(created
            ? $"Stack '{name}' has been created."
            : $"Failed to create stack with name '{name}'.");

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    internal void Delete()
    {
        Console.Clear();

        var stacks = _stackService.GetAll();

        if (stacks.Any())
        {
            TableVisualisation.ShowStacksTable(stacks);

            var name = UserInput.GetStringInput("Enter name of stack to delete.");
            var stackToDelete = _stackService.GetByName(name);

            if (stackToDelete.Id == 0)
            {
                Console.WriteLine($"No stack with name '{name}'.");
            }
            else
            {
                var deleted = _stackService.Delete(stackToDelete.Id);

                Console.WriteLine(deleted
                    ? $"Stack '{name}' has been deleted."
                    : $"Failed to delete stack with name '{name}'.");
            }
        }
        else
        {
            Console.WriteLine("No stacks to delete.");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}