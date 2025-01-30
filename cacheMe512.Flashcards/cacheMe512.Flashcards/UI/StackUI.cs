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
            HandleOption(choice);
        }
    }

    private void HandleOption(string option)
    {
        switch (option)
        {
            case "Add Stack":
                AddStack(_stackController);
                break;
            case "View Stacks":
                ViewStacks(_stackController);
                break;
            case "Delete Stack":
                DeleteStack(_stackController);
                break;
            case "Back to Main Menu":
                return;
            default:
                AnsiConsole.MarkupLine("Invalid option. Please try again.");
                break;
        }
    }

    private void AddStack(StackController stackController)
    {
        AnsiConsole.MarkupLine("Enter stack name: ");
        string name = Console.ReadLine();

        var newStack = new Stack
        {
            Name = name,
            CreatedDate = DateTime.Now
        };

        _stackController.InsertStack(newStack);

        AnsiConsole.MarkupLine("Stack added successfully!");
    }

    private void DeleteStack(StackController stackController)
    {
        var stacksToDelete = stackController.GetAllStacks();
        if(!stacksToDelete.Any())
        {
            Utilities.DisplayMessage("No stacks available to delete.[/]", "red");
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
            if (stackController.DeleteStack(stackToDelete.Id))
            {
                Utilities.DisplayMessage("Session deleted successfully!");
            }
            else
            {
                Utilities.DisplayMessage("Session not found.", "red");
            }
        }
        else
        {
            Utilities.DisplayMessage("Deletion canceled.");
        }

        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    private void ViewStacks(StackController stackController)
    {
        var stacks = stackController.GetAllStacks();
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
