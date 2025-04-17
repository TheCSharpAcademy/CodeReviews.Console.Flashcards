using Flashcards.DAL.Model;
using Flashcards.DAL;
using Spectre.Console;
using System;
using Flashcards.DAL.DTO;
using Microsoft.IdentityModel.Tokens;

namespace Flashcards
{
    public class UserInput
    {
        private readonly Controller _controller;

        public UserInput(Controller controller)
        {
            _controller = controller;
        }

        public void GetUserInput()
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold purple on black]Welcome to the Flashcards application![/]");

            while (1 == 1)
            {
                AnsiConsole.MarkupLine("[bold purple on black]MAIN MENU[/]");
                AnsiConsole.MarkupLine("[italic hotpink3_1 on black]Please choose a sub menu:[/]");
                AnsiConsole.MarkupLine("[italic hotpink3_1 on black]0) Exit Application[/]");
                AnsiConsole.MarkupLine("[italic hotpink3_1 on black]1) Flashcards[/]");
                AnsiConsole.MarkupLine("[italic hotpink3_1 on black]2) Stacks[/]");
                AnsiConsole.MarkupLine("[italic hotpink3_1 on black]3) Sessions (WIP)[/]");
                string input = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[italic hotpink3_1 on black]Please type one of the following values only:[/]")
                    .AddChoices([
                        "0",
                        "1",
                        "2",
                        "3"
                    ]));

                switch (input)
                {
                    case "0":
                        Environment.Exit(0);
                        break;
                    case "1":
                        GetFlashcardMenu();
                        break;
                    case "2":
                        GetStackMenu();
                        break;
                    case "3":
                        break;
                }
            }
        }

        private void GetFlashcardMenu()
        {
            AnsiConsole.MarkupLine("[bold purple on black]FLASHCARD MENU[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]Please choose an action:[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]0) Back to Main Menu[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]1) Create Flashcard[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]2) Delete Flashcard[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]3) Update Flashcard[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]4) Get a specific Flashcard[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]5) Get all Flashcards[/]");

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
                    CreateFlashcard();
                    break;
                case "2":
                    DeleteFlashcard();
                    break;
                case "3":
                    UpdateFlashcard();
                    break;
                case "4":
                    GetFlashcardById();
                    break;
                case "5":
                    //GetAllFlashcards();
                    break;
                default:
                    Console.WriteLine("Invalid command!");
                    break;
            }
        }

        private void GetStackMenu()
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
                    //GetStackByName();
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

        private void CreateFlashcard()
        {
            AnsiConsole.MarkupLine("[darkcyan]Please enter the front side text of your flashcard:[/]");
            string front = Console.ReadLine();
            AnsiConsole.MarkupLine("[darkcyan]Please enter the back side text of your flashcard:[/]");
            string back = Console.ReadLine();
            AnsiConsole.MarkupLine("[darkcyan]Please enter the stack name to which this flashcard will belong.[/]");
            string stackName = Console.ReadLine();

            if (_controller.CreateFlashcard(front, back ,stackName))
                AnsiConsole.MarkupLine("[white on green]Flashcard created.[/]");
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to create flashcard.[/]");
        }

        private void CreateFlashcard(string stackName)
        {
            AnsiConsole.MarkupLine("[darkcyan]Please enter the front side text of your flashcard:[/]");
            string front = Console.ReadLine();
            AnsiConsole.MarkupLine("[darkcyan]Please enter the back side text of your flashcard:[/]");
            string back = Console.ReadLine();

            if (_controller.CreateFlashcard(front, back, stackName))
                AnsiConsole.MarkupLine("[white on green]Flashcard created.[/]");
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to create flashcard.[/]");
        }

        private void CreateStack()
        {
            AnsiConsole.MarkupLine("[darkcyan]Please enter the name of your stack:[/]");
            string name = Console.ReadLine();

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

                if (input == "Yes") CreateFlashcard(name);
            }
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to create stack.[/]");
        }

        private void DeleteFlashcard()
        {
            AnsiConsole.MarkupLine("[darkcyan]Please enter the ID of the flashcard you would like to delete[/]");
            int id = Convert.ToInt32(Console.ReadLine);
            // Flashcard existingFlashcard = _validation.GetExistingFlashcard(); DONT MAKE THIS A MODEL JUST A BOOL

            if (_controller.DeleteFlashcard(id))
                AnsiConsole.MarkupLine("[white on green]Flashcard deleted.[/]");
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to delete flashcard.[/]");
        }

        private void DeleteStack()
        {
            AnsiConsole.MarkupLine("[darkcyan]Please enter the name of the stack you would like to delete[/]");
            string name = Console.ReadLine();
            // Flashcard existingFlashcard = _validation.GetExistingFlashcard(); DONT MAKE THIS A MODEL JUST A BOOL

            if (_controller.DeleteStack(name))
                AnsiConsole.MarkupLine("[white on green]Stack deleted.[/]");
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to delete stack.[/]");
        }

        private void UpdateFlashcard()
        {
            AnsiConsole.MarkupLine("[darkcyan]Please enter the ID of the flashcard you would like to update[/]");
            // Flashcard existingFlashcard = _validation.GetExistingFlashcard();
            int id = Convert.ToInt32(Console.ReadLine());

            AnsiConsole.MarkupLine("[darkcyan]Please enter the front side text of your flashcard:[/]");
            string front = Console.ReadLine();
            AnsiConsole.MarkupLine("[darkcyan]Please enter the back side text of your flashcard:[/]");
            string back = Console.ReadLine();
            AnsiConsole.MarkupLine("[darkcyan]Please enter the stack name to which this flashcard will belong.[/]");
            string stackName = Console.ReadLine();

            if (_controller.UpdateFlashcard(front, back, stackName))
                AnsiConsole.MarkupLine("[white on green]Flashcard updated.[/]");
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to update flashcard.[/]");
        }

        private void UpdateStack()
        {
            AnsiConsole.MarkupLine("[darkcyan]Please enter the name of the stack you would like to update[/]");
            // Flashcard existingFlashcard = _validation.GetExistingFlashcard();
            string currentName = Console.ReadLine();

            AnsiConsole.MarkupLine("[darkcyan]Please enter the new name of the stack:[/]");
            string updatedName = Console.ReadLine();

            if (_controller.UpdateStack(currentName, updatedName))
                AnsiConsole.MarkupLine("[white on green]Stack updated.[/]");
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to update stack.[/]");
        }

        private void GetFlashcardById()
        {
            AnsiConsole.MarkupLine("[darkcyan]Please enter the ID of the flashcard you would like to find.[/]");
            int id = Convert.ToInt32(Console.ReadLine());

            FlashcardStackDTO flashcard = _controller.GetFlashCardByID(id);
            if (flashcard != null)
                AnsiConsole.MarkupLine($"[white on green]Flashcard with ID of {flashcard.ID} \n\n Front text: {flashcard.Front} \n\n Back text: {flashcard.Back}[/]");
            else
                AnsiConsole.MarkupLine($"[white on red]Flashcard with ID of {id} does not exist.[/]");
        }

        //TODO: will show flashcards of stack, otherwise there's no point
        //private void GetStackByName()
        //{
        //    AnsiConsole.MarkupLine("[darkcyan]Please enter the name of the stack you would like to find.[/]");
        //    string name = Console.ReadLine();

        //    StackDTO stack = _controller.GetStackByName(name);
        //    if (stack != null)
        //        AnsiConsole.MarkupLine($"[white on green]Stack \"{stack.Name}\" has x amount of flashcards]");
        //    else
        //        AnsiConsole.MarkupLine($"[white on red]Flashcard with ID of {id} does not exist.[/]");
        //}

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
