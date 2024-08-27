using System.Runtime.InteropServices;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace FlashCards;

public class FlashCardsController
{
    private readonly ILogger _logger;
    private readonly IConfigurationRoot _config;
    private readonly SpectreConsole _console;
    private readonly DapperHelper _dapper;

    public FlashCardsController(ILogger logger, IConfigurationRoot config, SpectreConsole console, DapperHelper dapper)
    {
        _logger = logger;
        _config = config;
        _console = console;
        _dapper = dapper;
    }

    public void Run()
    {
        string[] mainMenu = {
            "Add Stack",
            "Delete Stack",
            "Add Card",
            "Delete Card",
            "Study Session",
            "View Study Sessions by Stack",
            "Average Score Yearly Report",
            "Exit"};
        
        // Main menu
        while (true)
        {
            switch (_console.MainMenu(mainMenu))
            {
                case "Add Stack":
                    var name = GetNewStackName();
                    AddStack(name);
                    break;
                case "Delete Stack":
                    DeleteStack();
                    break;
                case "Add Card":
                    AddCard();
                    break;
                case "Delete Card":
                    DeleteCard();
                    break;
                case "Start Study Session":
                    break;
                case "View Study Sessions by Stack":
                    break;
                case "Average Score Yearly Report":
                    break;
                case "Exit":
                    return;
            }
        }
    }

    private string GetNewStackName(List<string>? stackNames = null)
    {
        stackNames ??= _dapper.GetStacks().ConvertAll(stack => stack.Name);
        
        var stackName = _console.AddStackMenu(stackNames);

        return stackName;
    }

    private bool AddStack(string stackName)
    {
        if (stackName.Equals("0"))
            return false;
        
        if (_dapper.AddStack(stackName)) {
            _console.Success($"Stack {stackName} added");
            return true;
        }
        
        _console.Error("Something went wrong and stack could not be added.");
        return false;
    }

    private void DeleteStack()
    {
        var stacks = _dapper.GetStacks();
        var stack = _console.SelectStackMenu(stacks, "Which stack would you like to delete?");

        if (_console.Confirm($"Are you sure you want to delete the stack \"{stack.Name}\"?"))
        {
            if (_dapper.DeleteStack(stack.Id))
            {
                _console.Success("Stack Deleted!");
            }
            else
            {
                _console.Error("Something went wrong while deleting the stack.");
            }
        }
        else
        {
            _console.Success("Delete request cancelled.");
        }
    }

    private void AddCard()
    {
        var newStack = _console.Confirm("Create a new stack?");
        var stacks = _dapper.GetStacks();
        string stackName;

        if (newStack)
        {
            stackName = GetNewStackName(stacks.ConvertAll(stack => stack.Name));
            if (!AddStack(stackName))
                return;
        }
        else
        {
            stackName = _console.SelectStackMenu(stacks, "Which stack would you like to add?").Name;
        }

        var (front, back) = _console.NewCardMenu();

        if (_dapper.AddCard(front, back, stackName))
        {
            _console.Success($"Card added to {stackName}!");
        }
        else
        {
            _console.Error("Something went wrong and card could not be added.");
        }
    }

    private void DeleteCard()
    {
        var cards = _dapper.GetCardDtos();
        cards.Sort((here, there) => string.Compare(here.Name, there.Name, StringComparison.Ordinal));
        
        var card = _console.DeleteCardMenu(cards, "Which card do you wish to delete?");

        if (!_console.Confirm($"Are you sure you want to delete the card \"{card.Front}\"?"))
            return;
        
        if (_dapper.DeleteCard(card.Id))
        {
            _console.Success("Card deleted!");
        }
        else
        {
            _console.Error("Something went wrong and card could not be deleted.");
        }

    }
}