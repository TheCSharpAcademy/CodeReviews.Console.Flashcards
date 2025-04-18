using Flashcards.DAL.Model;
using Flashcards.DAL;
using Spectre.Console;
using System;
using Flashcards.DAL.DTO;
using Microsoft.IdentityModel.Tokens;

namespace Flashcards.UserInput
{
    public class UserInput
    {
        private readonly Controller _controller;
        private readonly FlashcardMenu flashcardMenu;
        private readonly StackMenu stackMenu;

        public UserInput(Controller controller)
        {
            _controller = controller;
            flashcardMenu = new FlashcardMenu(_controller);
            stackMenu = new StackMenu(_controller, flashcardMenu);
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
                        flashcardMenu.GetFlashcardMenu();
                        break;
                    case "2":
                        stackMenu.GetStackMenu();
                        break;
                    case "3":
                        break;
                }
            }
        }
    }
}
