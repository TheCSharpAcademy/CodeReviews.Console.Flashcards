using Flashcards.DAL.DTO;
using Flashcards.DAL;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace Flashcards.UserInput
{
    public class StackMenu : BaseMenu
    {
        private readonly FlashcardMenu _flashcardMenu;

        public StackMenu(Controller controller, Validation validation, FlashcardMenu flashcardMenu) : base(controller, validation)
        {
            _flashcardMenu = flashcardMenu;
        }

        public void GetStackMenu()
        {
            AnsiConsole.MarkupLine("[bold purple on black]Stack MENU[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]Please choose an action:[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]0) Back to Main Menu[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]1) Create Stack[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]2) Delete Stack[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]3) Update Stack[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]4) Get a specific Stack[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]5) Get all Stacks[/]");

            string input = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[italic hotpink3_1 on black]Please type one of the following values only:[/]")
                .AddChoices([
                        "0",
                        "1",
                        "2",
                        "3",
                        "4",
                        "5"
                ]));

            switch (input)
            {
                case "0":
                    break;
                case "1":
                    CreateStack();
                    break;
                case "2":
                    DeleteStack();
                    break;
                case "3":
                    UpdateStack();
                    break;
                case "4":
                    GetStackByName();
                    break;
                case "5":
                    GetAllStacks();
                    break;
                case "6":
                    break;
                default:
                    Console.WriteLine("Invalid command!");
                    break;
            }
        }

        private void CreateStack()
        {
            string name = _validation.GetUniqueStackName("[darkcyan]Please enter name of the stack you would like to create:[/]");

            if (_controller.CreateStack(name))
            {
                AnsiConsole.MarkupLine("[white on green]Stack created.[/]");

                string input = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[white on green]Would you like to add a flashcard to this empty stack?[/]")
                    .AddChoices([
                        "Yes",
                        "No"
                    ]));

                if (input == "Yes") _flashcardMenu.CreateFlashcard(name);
            }
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to create stack.[/]");
        }

        private void DeleteStack()
        {
            string name = _validation.GetExistingStackName("[darkcyan]Please enter the name of the stack you would like to delete[/]");

            if (_controller.DeleteStack(name))
                AnsiConsole.MarkupLine("[white on green]Stack deleted.[/]");
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to delete stack.[/]");
        }

        private void UpdateStack()
        {
            string currentName = _validation.GetExistingStackName("[darkcyan]Please enter the name of the stack you would like to update[/]");

            string updatedName = _validation.GetUniqueStackName("[darkcyan]Please enter the new name of the stack:[/]");

            if (_controller.UpdateStack(currentName, updatedName))
                AnsiConsole.MarkupLine("[white on green]Stack updated.[/]");
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to update stack.[/]");
        }

        private void GetStackByName()
        {
            string name = _validation.GetExistingStackName("[darkcyan]Please enter the name of the stack you would like to inspect[/]");
        }

        private void GetAllStacks()
        {
            List<StackDTO> stacks = _controller.GetAllStacks();
            if (!stacks.IsNullOrEmpty())
            {
                foreach (StackDTO stack in stacks)
                {
                    AnsiConsole.MarkupLine($"[white on green]Stack \"{stack.Name}\"[/]");
                }
            }
            else
                AnsiConsole.MarkupLine($"[white on red]No stacks found![/]");
        }
    }
}
