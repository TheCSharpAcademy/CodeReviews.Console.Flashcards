using Spectre.Console;
using System;

namespace Flashcards
{
    public class UserInput
    {
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
                    GetAllFlashcards();
                    break;
                case "6":
                    GetAllFlashcardsByDateRange();
                    break;
                case "7":
                    GetTotalDurationByDateRange();
                    break;
                case "8":
                    GetAverageDurationByDateRange();
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
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]5) Get all Stack[/]");
            AnsiConsole.MarkupLine("[italic hotpink3_1 on black]6) Find out how many hours do you need to complete a Stack.[/]");

            string input = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[italic hotpink3_1 on black]Please type one of the following values only:[/]")
                .AddChoices([
                    "0",
                        "1",
                        "2",
                        "3",
                        "4",
                        "5",
                        "6"
                ]));

            switch (input)
            {
                case "0":
                    break;
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
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
            DateTime startDateTime = _validation.GetValidatedDateTimeValue();
            AnsiConsole.MarkupLine("[darkcyan]Please enter the back side text of your flashcard:[/]");
            DateTime endDateTime = _validation.GetValidatedDateTimeValue();
            AnsiConsole.MarkupLine("[darkcyan]Please enter the stack name to which this flashcard will belong.[/]");
            double flashcard = _validation.GetValidatedDouble();

            if (_controller.CreateFlashcard(startDateTime, endDateTime, flashcard))
                AnsiConsole.MarkupLine("[white on green]Flashcard created.[/]");
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to create flashcard.[/]");
        }

        private void DeleteFlashcard()
        {
            AnsiConsole.MarkupLine("[darkcyan]Please enter the ID of the flashcard you would like to delete[/]");
            Flashcard existingFlashcard = _validation.GetExistingFlashcard();

            if (_controller.Delete(existingFlashcard))
                AnsiConsole.MarkupLine("[white on green]Flashcard deleted.[/]");
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to delete flashcard.[/]");
        }

        private void UpdateFlashcard()
        {
            AnsiConsole.MarkupLine("[darkcyan]Please enter the ID of the flashcard you would like to update[/]");
            Flashcard existingFlashcard = _validation.GetExistingFlashcard();

            AnsiConsole.MarkupLine("[darkcyan]Please enter updated StartDateTime[/]");
            DateTime startDateTime = _validation.GetValidatedDateTimeValue();
            AnsiConsole.MarkupLine("[darkcyan]Please enter updated EndDateTime[/]");
            DateTime endDateTime = _validation.GetValidatedDateTimeValue();
            AnsiConsole.MarkupLine("[darkcyan]Please enter updated Target Duration[/]");
            double targetDuration = _validation.GetValidatedDouble();

            if (_controller.UpdateFlashcard(existingFlashcard, startDateTime, endDateTime, targetDuration))
                AnsiConsole.MarkupLine("[white on green]Flashcard updated.[/]");
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to update flashcard.[/]");
        }

        private void GetFlashcardById()
        {
            AnsiConsole.MarkupLine("[darkcyan]Please enter the ID of the flashcard you would like to find.[/]");
            int id = _validation.GetValidatedInteger();

            Flashcard flashcard = _controller.GetById<Flashcard>(id, "flashcard");
            if (flashcard != null)
                AnsiConsole.MarkupLine($"[white on green]Flashcard with ID of {flashcard.Id} begins at: {flashcard.StartDateTime} and ends at: {flashcard.EndDateTime}. The target duration of the flashcard is {flashcard.TargetDuration} hours.[/]");
            else
                AnsiConsole.MarkupLine($"[white on red]Coding session with ID of {id} does not exist.[/]");
        }

        private void GetAllFlashcards()
        {
            AnsiConsole.MarkupLine("[bold springgreen2]Please find below a list of all your coding sessions.[/]");
            List<CodingSession> codingSessions = _controller.GetAll<CodingSession>("codingSession");
            if (codingSessions != null)
            {
                foreach (CodingSession c in codingSessions)
                {
                    AnsiConsole.MarkupLine($"[springgreen2]ID: {c.Id} - You had a coding session of {c.Duration:0.##} hours from {c.StartDateTime} to {c.EndDateTime}[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[white on red]No coding sessions found![/]");
            }
        }
    }
}
