using Flashcards.wkktoria.Models.Dtos;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.UserInteractions;
using Flashcards.wkktoria.UserInteractions.Helpers;
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
            UserOutput.InfoMessage("No stacks found.");

        ConsoleHelpers.PressToContinue();
    }

    internal void Create()
    {
        Console.Clear();

        var name = UserInput.GetStringInput("Enter name for new stack.");

        while (!StackValidator.CheckName(name))
        {
            UserOutput.ErrorMessage($"'{name}' is invalid name.");
            name = UserInput.GetStringInput("Enter name for new stack.");
        }

        while (_stackService.CheckIfNameExists(name))
        {
            UserOutput.ErrorMessage($"Stack with name '{name}' already exists.");
            name = UserInput.GetStringInput("Enter name for new stack.");
        }

        var newStack = new StackDto
        {
            Name = name
        };

        var created = _stackService.Create(newStack);

        if (created)
            UserOutput.SuccessMessage($"Stack '{name}' has been created.");
        else
            UserOutput.ErrorMessage($"Failed to create stack with name '{name}'.");

        ConsoleHelpers.PressToContinue();
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
                UserOutput.ErrorMessage($"No stack with name '{name}'.");
            }
            else
            {
                var deleted = _stackService.Delete(stackToDelete.Id);

                if (deleted)
                    UserOutput.SuccessMessage($"Stack '{name}' has been deleted.");
                else
                    UserOutput.ErrorMessage($"Failed to delete stack with name '{name}'.");
            }
        }
        else
        {
            UserOutput.InfoMessage("No stacks to delete.");
        }

        ConsoleHelpers.PressToContinue();
    }
}