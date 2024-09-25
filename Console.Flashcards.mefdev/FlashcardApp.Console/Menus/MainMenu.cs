using Spectre.Console;
using FlashcardApp.Core.Services.Interfaces;
using FlashcardApp.Console.MessageLoggers;

namespace FlashcardApp.Console.Menus;

public class MainMenu
{
    private readonly IStackService _stackService;

    public MainMenu(IStackService stackService)
    {
        _stackService = stackService;
    }

    public async Task DisplayMenu()
    {
        RenderCustomLine("DodgerBlue1", "Flashcard Menu");
        var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Select a [blue]function[/]?")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .AddChoices(new[] {
                "Add a Stack", "Delete A stack", "Add A Flashcard",
                "Delete A Flashcard", "Study Session", "View Study Sessions By Stack",
                "Average Score Yearly Report", "Exit",
            }));
        await DisplayOtherMenus(choice);
    }

    private static void RenderCustomLine(string color, string title)
    {
        var rule = new Rule($"[{color}]{title}[/]");
        rule.RuleStyle($"{color} dim");
        AnsiConsole.Write(rule);
    }

    private async Task AddStack(string stackName)
    {
        var result = await _stackService.AddStack(stackName);
        if (!result.IsSuccess)
        {
            MessageLogger.DisplayErrorMessage("The Stack is already exists.");
            return;
        }
        MessageLogger.DisplaySuccessMessage("[green]1[/] stack added.");
        AnsiConsole.Prompt(new TextPrompt<string>("Press any Key to continue").AllowEmpty());
    }

    private async Task  DisplayAddStackMenu()
    {
        var stackName = AnsiConsole.Prompt(
        new TextPrompt<string>("Please enter a [blue]stack name[/] or enter 0 to return to [red]main menu: [/]"));
        if (!stackName.Equals("0"))
        {
            await AddStack(stackName);
        }
        await DisplayMenu();   
    }

    private string DisplayDeleteStackMenu()
    {
       var choice = AnsiConsole.Prompt(
       new SelectionPrompt<string>()
           .Title("Select a [red]stack[/] to delete")
           .PageSize(10)
           .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
           .AddChoices(new[] {
                "stack 1", "stack 2", "stack 3",
           }));
        AnsiConsole.Prompt(new TextPrompt<string>("[red]1[/] stack deleted. Press any Key to continue").AllowEmpty());
        return choice;
    }

    private async Task DisplayOtherMenus(string choice)
    {
        choice = choice.ToLower().Trim();
        switch (choice)
        {
            case "add a stack":
                   await DisplayAddStackMenu();
                break;
            case "delete a stack":
                   DisplayDeleteStackMenu();
                break;
            default:
                break;
                
        }
    }
}