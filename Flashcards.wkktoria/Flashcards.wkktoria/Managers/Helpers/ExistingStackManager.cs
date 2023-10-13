using Flashcards.wkktoria.Controllers;
using Flashcards.wkktoria.Models;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.UserInteractions;

namespace Flashcards.wkktoria.Managers.Helpers;

internal class ExistingStackManager
{
    private readonly CardController _cardController;
    private readonly StackService _stackService;
    private Stack? _currentStack;

    internal ExistingStackManager(StackService stackService, CardService cardService)
    {
        _stackService = stackService;
        _cardController = new CardController(cardService);
    }

    private void ShowMenu()
    {
        var currentStackName = _currentStack == null ? "-" : _currentStack.Name;

        UserOutput.InfoMessage($"""
                                Current stack: {currentStackName}

                                0 - Return
                                1 - Change current stack
                                2 - Show all cards in current stack
                                3 - Show X cards in current stack
                                4 - Create card in current current stack
                                5 - Update card in current stack
                                6 - Delete card in current stack
                                """);
    }

    internal void Run()
    {
        var quit = false;

        if (!_stackService.GetAll().Any())
        {
            UserOutput.InfoMessage("No existing stacks.");
            UserOutput.InfoMessage("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        while (_currentStack == null)
        {
            Console.Clear();
            ChangeCurrent();
        }

        while (!quit)
        {
            Console.Clear();
            ShowMenu();

            Console.Write("> ");
            var option = Console.ReadLine();

            switch (option!.Trim())
            {
                case "0":
                    quit = true;
                    break;
                case "1":
                    ChangeCurrent();
                    break;
                case "2":
                    _cardController.ShowCards(_currentStack.Id);
                    break;
                case "3":
                    _cardController.ShowXCards(_currentStack.Id);
                    break;
                case "4":
                    _cardController.Create(_currentStack.Id);
                    break;
                case "5":
                    _cardController.Update(_currentStack.Id);
                    break;
                case "6":
                    _cardController.Delete(_currentStack.Id);
                    break;
                default:
                    UserOutput.ErrorMessage("Invalid option.");
                    UserOutput.InfoMessage("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void ChangeCurrent()
    {
        var stacks = _stackService.GetAll();

        if (stacks.Any())
        {
            TableVisualisation.ShowStacksTable(stacks);

            var name = UserInput.GetStringInput("Enter name of stack to use.");
            var stack = _stackService.GetByName(name);

            if (stack.Id != 0)
            {
                _currentStack = stack;
                UserOutput.InfoMessage($"Current stack has been changed to '{stack.Name}'.");
            }
            else
            {
                UserOutput.ErrorMessage($"No stack with name '{name}'.");
            }
        }
        else
        {
            UserOutput.InfoMessage("No stacks found.");
        }

        UserOutput.InfoMessage("Press any key to continue...");
        Console.ReadKey();
    }
}