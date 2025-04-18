using Flashcards.DAL;
using Flashcards.DAL.DTO;
using Spectre.Console;

namespace Flashcards.UserInput
{
    public class FlashcardMenu : BaseMenu
    {
        public FlashcardMenu(Controller controller, Validation validation) : base(controller, validation)
        {
        }

        public void GetFlashcardMenu()
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
            string stackName = _validation.GetExistingStackName("[darkcyan]Please enter the stack name to which this flashcard will belong.[/]");

            if (_controller.CreateFlashcard(front, back, stackName))
                AnsiConsole.MarkupLine("[white on green]Flashcard created.[/]");
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to create flashcard.[/]");
        }

        private void DeleteFlashcard()
        {
            int id = _validation.GetExistingFlashcardID("[darkcyan]Please enter the ID of the flashcard you would like to delete[/]");

            if (_controller.DeleteFlashcard(id))
                AnsiConsole.MarkupLine($"[white on green]Flashcard with ID of {id} deleted.[/]");
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to delete flashcard.[/]");
        }

        private void UpdateFlashcard()
        {
            int id = _validation.GetExistingFlashcardID("[darkcyan]Please enter the ID of the flashcard you would like to update[/]");

            AnsiConsole.MarkupLine("[darkcyan]Please enter the front side text of your flashcard:[/]");
            string front = Console.ReadLine();
            AnsiConsole.MarkupLine("[darkcyan]Please enter the back side text of your flashcard:[/]");
            string back = Console.ReadLine();
            string stackName = _validation.GetExistingStackName("[darkcyan]Please enter the stack name to which this flashcard will belong.[/]");

            if (_controller.UpdateFlashcard(id, front, back, stackName))
                AnsiConsole.MarkupLine("[white on green]Flashcard updated.[/]");
            else
                AnsiConsole.MarkupLine("[white on red]Something went wrong, unable to update flashcard.[/]");
        }

        private void GetFlashcardById()
        {
            int id = _validation.GetExistingFlashcardID("[darkcyan]Please enter the ID of the flashcard you would like to find.[/]");

            FlashcardStackDto flashcard = _controller.GetFlashCardByID(id);

            if (flashcard != null)
                AnsiConsole.MarkupLine($"[white on green]Flashcard with ID of {flashcard.ID} \n Front text: {flashcard.Front} \n Back text: {flashcard.Back} \n Stack: {flashcard.StackName}[/]");
            else
                AnsiConsole.MarkupLine($"[white on red]Flashcard with ID of {id} does not exist.[/]");
        }

        private void GetAllFlashcards()
        {
            List<FlashcardStackDto> flashcards = _controller.GetAllFlashcards();
            foreach (FlashcardStackDto flashcard in flashcards)
            {
                AnsiConsole.MarkupLine($"[white on green]Flashcard with ID of {flashcard.ID} \n Front text: {flashcard.Front} \n Back text: {flashcard.Back} \n Stack: {flashcard.StackName}[/]");
            }
        }
    }
}
