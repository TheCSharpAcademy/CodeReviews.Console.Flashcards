using Flashcards.empty_codes.Controllers;
using Flashcards.empty_codes.Models;
using Spectre.Console;

namespace Flashcards.empty_codes.Views
{
    internal class StackMenu
    {
        public StacksController StackController { get; }
        public MainMenu MainMenu { get; }
        public FlashcardMenu FlashcardMenu { get; }
        public void GetStackMenu()
        {
            ViewAllStacks();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose an [green]option below[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal your choices)[/]")
                    .AddChoices(new[] {
                        "Select a Stack", "Add New Stack", "Update Stack",
                        "Delete Stack", "Return to Main Menu",
                    }));

            switch (choice)
            {
                case "Select a Stack":
                    SelectAStack();
                    break;
                case "Add New Stack":
                    AddNewStack();
                    break;
                case "Update Stack":
                    UpdateStack();
                    break;
                case "Delete Stack":
                    DeleteStack();
                    break;
                case "Return to Main Menu":
                    MainMenu.GetMainMenu();
                    break;
                default:
                    AnsiConsole.WriteLine("Invalid selection. Please try again.");
                    break;
            }

        }

        public void SelectAStack()
        {
            ViewAllStacks();
            var name = AnsiConsole.Ask<string>("Enter the name of the stack you want to select: ");
            StackDTO stack = new StackDTO();
            stack.StackName = name;
            if (StackController.CheckIfStackExists(stack) > 0)
            {
                AnsiConsole.WriteLine($"Current stack: {stack.StackName}");
                var stackChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose an [green]option below[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal your choices)[/]")
                    .AddChoices(new[] {
                        "Select another Stack", "View all flashcards in this stack", "Create a new flashcard in this stack",
                        "Edit a flashcard", "Delete a flashcard", "Return to Main Menu",
                    }));

                switch (stackChoice)
                {
                    case "Select another Stack":
                        SelectAStack();
                        break;
                    case "Manage flashcards":
                        FlashcardMenu.GetFlashcardMenu();
                        break;
                    case "Return to Main Menu":
                        MainMenu.GetMainMenu();
                        break;
                    default:
                        AnsiConsole.WriteLine("Invalid selection. Please try again.");
                        break;
                }
            }
            else
            {
                AnsiConsole.MarkupLine($"[yellow]No stack found with the provided name: {stack.StackName}[/]");
            }

        }

        public void AddNewStack()
        {
            var name = AnsiConsole.Ask<string>("Enter the stack name: ");
            StackDTO stack = new StackDTO();
            stack.StackName = name;
            StackController.InsertStack(stack);
        }

        public void ViewAllStacks()
        {
            var stacks = StackController.ViewAllStacks();
            if (stacks.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No stacks found![/]");
            }
            else
            {
                var table = new Table();
                table.Title = new TableTitle("All Stacks", Style.Parse("bold yellow"));
                table.AddColumn("[bold]Name[/]");

                foreach (var stack in stacks)
                {
                    table.AddRow(
                           stack.StackName
                       );
                }
                Console.Clear();
                AnsiConsole.Write(table);
            }

        }

        public void UpdateStack()
        {
            var name = AnsiConsole.Ask<string>("Enter the name of the stack you want to update: ");
            StackDTO stack = new StackDTO();
            stack.StackName = name;

            var newStackName = AnsiConsole.Ask<string>("Enter the new name of the stack: ");
            StackController.UpdateStack(stack, newStackName);
        }

        public void DeleteStack()
        {
            var name = AnsiConsole.Ask<string>("Enter the name of the stack you want to delete: ");
            StackDTO stack = new StackDTO();
            stack.StackName = name;

            var confirmation = AnsiConsole.Prompt(new ConfirmationPrompt("Are you sure?"));
            if (confirmation == true)
            {
                StackController.DeleteStack(stack);
            } 
            
        }
    }
}
