using Spectre.Console;
using cacheMe512.Flashcards.DTOs;
using cacheMe512.Flashcards.Controllers;
using cacheMe512.Flashcards.Models;

namespace cacheMe512.Flashcards.UI
{
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
            string name = AnsiConsole.Ask<string>("Enter stack name:");

            var newStack = new Stack
            {
                Name = name,
                Position = 0,
                CreatedDate = DateTime.Now

            };

            _stackController.InsertStack(newStack);

            Utilities.DisplayMessage("Stack added successfully!", "green");
            AnsiConsole.MarkupLine("\nPress Any Key to Continue.");
            Console.ReadKey();
        }

        private void ViewStacks()
        {
            var stacks = _stackController.GetAllStacks().ToList();
            if (!stacks.Any())
            {
                AnsiConsole.MarkupLine("[red]No stacks available. Add a stack first.[/]");
                AnsiConsole.MarkupLine("\nPress Any Key to Continue.");
                Console.ReadKey();
                return;
            }

            var selectedStack = AnsiConsole.Prompt(
                new SelectionPrompt<StackDto>()
                    .Title("[bold yellow]Select a Stack to Manage Flashcards[/]")
                    .PageSize(10)
                    .UseConverter(stack => $"{stack.Position}: {stack.Name}")
                    .AddChoices(stacks)
            );

            var flashcardUI = new FlashcardUI(selectedStack);
            flashcardUI.Show();
        }

        private void DeleteStack()
        {
            var stacksToDelete = _stackController.GetAllStacks().ToList();
            if (!stacksToDelete.Any())
            {
                Utilities.DisplayMessage("No stacks available to delete.", "red");
                AnsiConsole.MarkupLine("\nPress Any Key to Continue.");
                Console.ReadKey();
                return;
            }

            var stackToDelete = AnsiConsole.Prompt(
                new SelectionPrompt<StackDto>()
                    .Title("Select a stack to [red]delete[/]:")
                    .UseConverter(s => $"{s.Name}")
                    .AddChoices(stacksToDelete)
            );

            if (Utilities.ConfirmDeletion(stackToDelete))
            {
                var stack = _stackController.GetAllStacks().FirstOrDefault(s => s.Name == stackToDelete.Name);
                if (stack == null)
                {
                    Utilities.DisplayMessage("Stack not found.", "red");
                    AnsiConsole.MarkupLine("\nPress Any Key to Continue.");
                    Console.ReadKey();
                    return;
                }

                if (_stackController.DeleteStack(stack.Id))
                {
                    Utilities.DisplayMessage("Stack deleted successfully!", "green");
                }
                else
                {
                    Utilities.DisplayMessage("Failed to delete stack.", "red");
                }
            }
            else
            {
                Utilities.DisplayMessage("Deletion canceled.", "yellow");
            }

            AnsiConsole.MarkupLine("\nPress Any Key to Continue.");
            Console.ReadKey();
        }

    }
}
