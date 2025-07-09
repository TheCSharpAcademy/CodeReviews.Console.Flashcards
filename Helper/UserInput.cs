namespace DotNETConsole.Flashcards.Helper;

using Models;
using DTO;
using Spectre.Console;
using Controllers;
using Views;

public class UserInput
{
    private StackController _stackController = new StackController();
    private CardController _cardController = new CardController();
    private UserViews _userViews = new UserViews();
    public Stack? SelectSingleStack()
    {
        var stacks = _stackController.GetStacks();
        if (stacks.Count == 0)
        {
            _userViews.Tost("No Stack Found!!!", "info");
            return null;
        }
        var stack = AnsiConsole.Prompt(
            new SelectionPrompt<Stack>()
                .Title("Select habit:")
                .PageSize(10)
                .AddChoices(stacks));
        return stack;
    }

    public CardViewDto? SelectSingleCard()
    {
        var cards = _cardController.GetAllCardsView();

        if (cards.Count == 0)
        {
            _userViews.Tost("No Card Found!!!", "info");
            return null;
        }

        var card = AnsiConsole.Prompt(
            new SelectionPrompt<CardViewDto>()
                .Title("Select card:")
                .PageSize(10)
                .AddChoices(cards));

        return card;
    }

    public string GetStackName()
    {
        string name = AnsiConsole.Prompt(
                    new TextPrompt<string>("Stack Name: ").Validate((title) => title.Trim().Length switch
                    {
                        < 3 => ValidationResult.Error("Blank or too short(min 3 char)."),
                        >= 3 => ValidationResult.Success()
                    }));
        return name;
    }

    public (string, string) GetCardInfo()
    {
        string question = AnsiConsole.Prompt(
                     new TextPrompt<string>("Question: ").Validate((input) => input.Trim().Length switch
                     {
                         < 5 => ValidationResult.Error("Blank or too short(min 5 char)."),
                         >= 5 => ValidationResult.Success()
                     }));
        string answer = AnsiConsole.Prompt(
                     new TextPrompt<string>("Answer: ").Validate((input) => input.Trim().Length switch
                     {
                         < 1 => ValidationResult.Error("Can not be Blank."),
                         >= 1 => ValidationResult.Success()
                     }));
        return (question, answer);
    }

    public bool ContinueInput(string? extraMessage = null)
    {
        if (extraMessage != null)
        {
            AnsiConsole.MarkupLineInterpolated($"\n[blue]{extraMessage}...[/]");
        }
        AnsiConsole.MarkupLine("[green]Press ESC to continue...[/]");
        while (true)
        {
            var keyPressed = AnsiConsole.Console.Input.ReadKey(true);
            if (keyPressed is { Key: ConsoleKey.Escape })
            {
                return true;
            }
        }
    }

    public ConsoleKeyInfo? ModifyOptionPrompt()
    {
        AnsiConsole.MarkupLine("[green]> Press ESC to return to main menu\n> Press E to edit\n> Press D to delete...[/]");
        while (true)
        {
            var keyPressed = AnsiConsole.Console.Input.ReadKey(true);
            if (keyPressed is { Key: ConsoleKey.Escape } || keyPressed is { Key: ConsoleKey.D } || keyPressed is { Key: ConsoleKey.E })
            {
                return keyPressed;
            }
        }
    }
}
