using Spectre.Console;
using FlashcardApp.Core.Services.Interfaces;
using FlashcardApp.Console.MessageLoggers;
using FlashcardApp.Core.Models;

namespace FlashcardApp.Console.Menus;

public class MainMenu
{
    private readonly IStackService _stackService;
    private readonly IFlashcardService _flashcardService;

    public MainMenu(IStackService stackService, IFlashcardService flashcardService)
    {
        _stackService = stackService;
        _flashcardService = flashcardService;
    }

    /******************************************** Main menu ********************************************/
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

    private async Task DisplayOtherMenus(string choice)
    {
        choice = choice.ToLower().Trim();
        switch (choice)
        {
            case "add a stack":
                await DisplayAddStackMenu();
                break;
            case "delete a stack":
                await DisplayDeleteStackMenu();
                break;
            case "add a flashcard":
                await DisplayAddFlashcardMenu();
                break;
            case "delete a flashcard":
                await DisplayAddFlashcardMenu();
                break;
            case "study session":
                await DisplayAddFlashcardMenu();
                break;
            case "view study sessions by stack":
                await DisplayAddFlashcardMenu();
                break;
            case "average score yearly report":
                await DisplayAddFlashcardMenu();
                break;
            default:
                Environment.Exit(0);
                break;
        }
    }

    private static void RenderCustomLine(string color, string title)
    {
        var rule = new Rule($"[{color}]{title}[/]");
        rule.RuleStyle($"{color} dim");
        AnsiConsole.Write(rule);
    }

    /******************************************** Stack Menu********************************************/
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

    private async Task<string[]> GetAllStackNames()
    {
        var stacks = await _stackService.GetAllStacks();
        string[] stackNames = stacks.Value.Select(stack => stack.Name).ToArray();
        return stackNames;
    }

    private async Task<Stack> GetStack(string stackName)
    {
       var stack = await _stackService.GetStackByName(stackName);
       return stack.Value;
    }

    private async Task DeleteStack(string stackName)
    {
        var result = await _stackService.DeleteStack(stackName);
        if (!result.IsSuccess)
        {
            MessageLogger.DisplayErrorMessage("The Stack cannot be deleted.");
            return;
        }
        MessageLogger.DisplaySuccessMessage("[green]1[/] stack is deleted.");
        AnsiConsole.Prompt(new TextPrompt<string>("Press any Key to continue").AllowEmpty());
    }

    private async Task DisplayAddStackMenu()
    {
        var stackName = AnsiConsole.Prompt(
        new TextPrompt<string>("Please enter a [blue]stack name[/] or enter 0 to return to [red]main menu: [/]"));
        if (!stackName.Equals("0"))
        {
            await AddStack(stackName);
        }
        await DisplayMenu();   
    }

    private async Task DisplayDeleteStackMenu()
    {
       var stackNames = await GetAllStackNames();
       var choice = AnsiConsole.Prompt(
       new SelectionPrompt<string>()
           .Title("Select a [red]stack[/] to delete")
           .PageSize(10)
           .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
           .AddChoices(stackNames));
        await DeleteStack(choice);
    }

    /******************************************** Flashcard Menu********************************************/
    private async Task DisplayAddFlashcardMenu()
    {
        var response = await CheckPrompt("The flashcard will be for a new stack(Y/N):  ");
        if (response.Equals("y"))
        {
            await DisplayAddStackMenu();
        }
        var stackNames = await GetAllStackNames();
        var flashCardStackName = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
           .Title("Select a [green]stack[/] to add the flashcard to")
           .PageSize(10)
           .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
           .AddChoices(stackNames));
        var question = await CheckPrompt("Please enter a the [green]front[/] of the flashcard or enter 0 to return to [red]main menu: [/]");
        var answer = await CheckPrompt("Please enter a the [green]back[/] of the flashcard or enter 0 to return to [red]main menu: [/]");

        var stack = await GetStack(flashCardStackName);
        await AddFlashcard(new Flashcard { Id = 1, Answer = answer, Question = question, stack = stack });
    }

    private async Task AddFlashcard(Flashcard flashcard)
    {   
        var result = await _flashcardService.AddFlashcard(flashcard);
        if (!result.IsSuccess)
        {
            MessageLogger.DisplayErrorMessage("The flashcard is already exists.");
            return;
        }
        MessageLogger.DisplaySuccessMessage("[green]1[/] flashcard added.");
        AnsiConsole.Prompt(new TextPrompt<string>("Press any Key to continue").AllowEmpty());
    }


    private async Task<string> CheckPrompt(string message)
    {
        string prompt = "";
        do
        {
            prompt = AnsiConsole.Prompt(
                new TextPrompt<string>(message).AllowEmpty());
            if (prompt.Equals("0")) await DisplayMenu();
            if (string.IsNullOrEmpty(prompt))
            {
                AnsiConsole.MarkupLine("[red]Empty value is not allowed[/]");

            }
        } while (string.IsNullOrEmpty(prompt));
        return prompt.ToLower();
    }
}