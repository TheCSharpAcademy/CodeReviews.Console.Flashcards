using Spectre.Console;
using Flashcards.empty_codes.Controllers;
using Flashcards.empty_codes.Models;

namespace Flashcards.empty_codes.Views
{
    internal class FlashcardMenu
    {
        public FlashcardsController FlashcardController { get; }
        
        public void GetFlashcardMenu(StackDTO stack)
        {
            ViewAllFlashcards(stack);
            var flashcardChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose an [green]option below[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal your choices)[/]")
                    .AddChoices(new[] {
                        "Add New Flashcard","Edit Flashcard",
                        "Delete Flashcard", "Return to Main Menu",
                    }));

            switch (flashcardChoice)
            {
                case "Add New Flashcard":
                    AddNewFlashcard(stack); 
                    break;
                case "Edit Flashcard":
                    UpdateFlashcard(stack); 
                    break;
                case "Delete Flashcard":
                    DeleteFlashcard(stack); 
                    break;
                case "Return to Main Menu":
                    GetFlashcardMenu(stack);
                    break;
                default:
                    AnsiConsole.WriteLine("Invalid selection. Please try again."); 
                    break;
            }

        }

        public void AddNewFlashcard(StackDTO stack)
        {
            var question = AnsiConsole.Ask<string>("Enter the question: ");
            var answer = AnsiConsole.Ask<string>("Enter the answer: ");
            FlashcardDTO card = new FlashcardDTO();
            card.Question = question;
            card.Answer = answer;
            card.StackId = stack.StackId;

            FlashcardController.InsertFlashcard(card);
        }

        public void UpdateFlashcard(StackDTO stack)
        {
            var editId = AnsiConsole.Ask<int>("Enter the id of the flashcard you want to edit: ");
            var newQuestion = AnsiConsole.Ask<string>("Enter the new question: ");
            var newAnswer = AnsiConsole.Ask<string>("Enter the new answer: ");
            FlashcardDTO card = new FlashcardDTO();
            card.FlashcardId = editId;
            card.Question = newQuestion;
            card.Answer = newAnswer;
            card.StackId = stack.StackId;

            FlashcardController.UpdateFlashcard(card);
        }

        public void DeleteFlashcard(StackDTO stack)
        {
            var deleteId = AnsiConsole.Ask<int>("Enter the id of the flashcard you want to edit: ");
            FlashcardDTO card = new FlashcardDTO();
            card.FlashcardId = deleteId;
            card.StackId = stack.StackId;

            var confirmation = AnsiConsole.Prompt(new ConfirmationPrompt("Are you sure?"));
            if (confirmation == true)
            {
                FlashcardController.DeleteFlashcard(card);
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Flashcard not deleted![/]");
                GetFlashcardMenu(stack);
            }

        }

        public void ViewAllFlashcards(StackDTO stack)
        {
            var cards = FlashcardController.ViewAllFlashcards(stack);
            if (cards.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No flashcards found![/]");
            }
            else
            {
                var table = new Table();
                table.Title = new TableTitle("All Flashcards", Style.Parse("bold yellow"));
                table.AddColumn("[bold]Id[/]");
                table.AddColumn("[bold]Question[/]");
                table.AddColumn("[bold]Answer[/]");

                foreach (var card in cards)
                {
                    table.AddRow(
                           card.FlashcardId,
                           card.Question,
                           card.Answer
                       );
                }
                Console.Clear();
                AnsiConsole.Write(table);
            }

        }
    }
}
