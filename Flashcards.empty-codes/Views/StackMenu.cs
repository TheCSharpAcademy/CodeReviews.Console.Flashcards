using Flashcards.empty_codes.Controllers;
using Flashcards.empty_codes.Models;
using Spectre.Console;

namespace Flashcards.empty_codes.Views
{
    internal class StackMenu
    {
        public void GetStackMenu()
        {
            MainMenu menu = new MainMenu();
            Console.Clear();
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
                    GetStackMenu();
                    break;
                case "Add New Stack":
                    AddNewStack();
                    GetStackMenu();
                    break;
                case "Update Stack":
                    UpdateStack();
                    GetStackMenu();
                    break;
                case "Delete Stack":
                    DeleteStack();
                    GetStackMenu();
                    break;
                case "Return to Main Menu":
                    menu.GetMainMenu();
                    break;
                default:
                    AnsiConsole.WriteLine("Invalid selection. Please try again.");
                    break;
            }

        }

        public void SelectAStack()
        {
            MainMenu menu = new MainMenu();
            StacksController stackController = new StacksController();
            ViewAllStacks();
            var name = AnsiConsole.Ask<string>("Enter the name of the stack you want to select: ");
            StackDTO stack = new StackDTO();
            stack.StackName = name;
            if (stackController.CheckIfStackExists(stack) > 0)
            {
                Console.Clear();
                AnsiConsole.WriteLine($"Current stack: {stack.StackName}");
                var stackChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose an [green]option below[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal your choices)[/]")
                    .AddChoices(new[] {
                        "Select another Stack", "Manage flashcards", "Return to Main Menu",
                    }));

                switch (stackChoice)
                {
                    case "Select another Stack":
                        SelectAStack();
                        break;
                    case "Manage flashcards":
                        FlashcardMenu flashcardMenu = new FlashcardMenu();
                        flashcardMenu.GetFlashcardMenu(stack);
                        SelectAStack();
                        break;
                    case "Return to Main Menu":
                        menu.GetMainMenu();
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
            StacksController stackController = new StacksController();
            stackController.InsertStack(stack);
            Console.ReadKey();
        }

        public void ViewAllStacks()
        {
            StacksController stackController = new StacksController();
            var stacks = stackController.ViewAllStacks();
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
            StacksController stackController = new StacksController();
            var name = AnsiConsole.Ask<string>("Enter the name of the stack you want to update: ");
            StackDTO stack = new StackDTO();
            stack.StackName = name;

            var newStackName = AnsiConsole.Ask<string>("Enter the new name of the stack: ");
            stackController.UpdateStack(stack, newStackName);
            Console.ReadKey();
        }

        public void DeleteStack()
        {
            StacksController stackController = new StacksController();
            var name = AnsiConsole.Ask<string>("Enter the name of the stack you want to delete: ");
            StackDTO stack = new StackDTO();
            stack.StackName = name;

            var confirmation = AnsiConsole.Prompt(new ConfirmationPrompt("Are you sure?"));
            if (confirmation == true)
            {
                stackController.DeleteStack(stack);
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Stack not deleted![/]");
            }
            Console.ReadKey();
        }
    }
}
