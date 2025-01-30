using Spectre.Console;
using cacheMe512.Flashcards.Models;
using cacheMe512.Flashcards.Controllers;

namespace cacheMe512.Flashcards.UI;

internal class StackUI
{
    private static readonly StackController _stackController = new();
    public void Show()
    {
        while (true)
        {
            Console.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]=== Manage Stacks ===[/]")
                    .PageSize(4)
                    .AddChoices(new[]
                    {
                            "Add Stack",
                            "View Stacks",
                            "Delete Stack",
                            "Back to Main Menu"
                    })
            );

            if (choice == "Back to Main Menu")
                return;

            HandleOption(choice);
        }
    }

    private void HandleOption(string option)
    {
        switch (option)
        {
            case "Add Stack":
                AddStack();
                break;
            case "View Stacks":
                ViewStacks();
                break;
            case "Delete Stack":
                DeleteStack();
                break;
        }
    }

    private void AddStack()
    {
        AnsiConsole.MarkupLine("Enter stack name: ");
        string name = Console.ReadLine();

        var newStack = new Stack
        {
            Name = name,
            CreatedDate = DateTime.Now
        };

        _stackController.InsertStack(newStack);

        Utilities.DisplayMessage("Stack added successfully!", "green");
        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    private void DeleteStack()
    {
        var stacksToDelete = _stackController.GetAllStacks();
        if(!stacksToDelete.Any())
        {
            Utilities.DisplayMessage("No stacks available to delete.", "red");
            AnsiConsole.MarkupLine("Press Any Key to Continue.");
            Console.ReadKey();
            return;
        }

        var stackToDelete = AnsiConsole.Prompt(
            new SelectionPrompt<Stack>()
                .Title("Select a stack to [red]delete[/]:")
                .UseConverter(s => $"{s.Name}")
                .AddChoices(stacksToDelete));

        if(Utilities.ConfirmDeletion(stackToDelete))
        {
            if (_stackController.DeleteStack(stackToDelete.Id))
            {
                Utilities.DisplayMessage("Stack deleted successfully!");
            }
            else
            {
                Utilities.DisplayMessage("Stack not found.", "red");
            }
        }
        else
        {
            Utilities.DisplayMessage("Deletion canceled.");
        }

        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    private void ViewStacks()
    {
        var stacks = _stackController.GetAllStacks();
        if (!stacks.Any())
        {
            AnsiConsole.MarkupLine("[red]No stacks available. Add a stack first.[/]");
            Console.ReadKey();
            return;
        }

        var selectedStack = AnsiConsole.Prompt(
            new SelectionPrompt<Stack>()
                .Title("[bold yellow]Select a Stack to Manage Flashcards[/]")
                .PageSize(10)
                .UseConverter(stack => $"{stack.Id}: {stack.Name}")
                .AddChoices(stacks)
        );

        var flashcardUI = new FlashcardUI(selectedStack);
        flashcardUI.Show();
    }

}
