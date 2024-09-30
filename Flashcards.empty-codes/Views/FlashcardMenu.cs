using Spectre.Console;
using Flashcards.empty_codes.Controllers;
using Flashcards.empty_codes.Models;

namespace Flashcards.empty_codes.Views;

internal class FlashcardMenu
{
    public void GetFlashcardMenu(StackDTO stack)
    {
        Console.Clear();
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
                GetFlashcardMenu(stack);
                break;
            case "Edit Flashcard":
                UpdateFlashcard(stack);
                GetFlashcardMenu(stack);
                break;
            case "Delete Flashcard":
                DeleteFlashcard(stack);
                GetFlashcardMenu(stack);
                break;
            case "Return to Main Menu":
                MainMenu menu = new MainMenu();
                menu.GetMainMenu();
                break;
            default:
                AnsiConsole.WriteLine("Invalid selection. Please try again."); 
                break;
        }
    }

    public void AddNewFlashcard(StackDTO stack)
    {
        FlashcardsController flashcardController = new FlashcardsController();
        var question = AnsiConsole.Ask<string>("Enter the question: ");
        var answer = AnsiConsole.Ask<string>("Enter the answer: ");
        FlashcardDTO card = new FlashcardDTO();
        card.Question = question;
        card.Answer = answer;
        card.StackId = stack.StackId;

        flashcardController.InsertFlashcard(card);
        Console.ReadKey();
    }

    public void UpdateFlashcard(StackDTO stack)
    {
        FlashcardsController flashcardController = new FlashcardsController();
        var oldQuestion = AnsiConsole.Ask<string>("Enter the question you want to change: ");
        int id = flashcardController.GetFlashcardIdByQuestion(oldQuestion, stack.StackId);

        if (id == -1)
        {
            AnsiConsole.MarkupLine("[red]Invalid question entered.[/]");
            Console.ReadKey();
            return; 
        }

        var newQuestion = AnsiConsole.Ask<string>("Enter the new question: ");
        var newAnswer = AnsiConsole.Ask<string>("Enter the new answer: ");

        FlashcardDTO card = new FlashcardDTO();
        card.FlashcardId = id;
        card.Question = newQuestion;
        card.Answer = newAnswer;
        card.StackId = stack.StackId;

        flashcardController.UpdateFlashcard(card);
        Console.ReadKey();
    }

    public void DeleteFlashcard(StackDTO stack)
    {
        FlashcardsController flashcardController = new FlashcardsController();
        var deleteQuestion = AnsiConsole.Ask<string>("Enter the question you want to delete: ");
        int id = flashcardController.GetFlashcardIdByQuestion(deleteQuestion, stack.StackId);
        if(id == -1)
        {
            AnsiConsole.MarkupLine("[red]Invalid question entered.[/]");
            Console.ReadKey();
            return;
        }
        FlashcardDTO card = new FlashcardDTO();
        card.FlashcardId = id;
        card.StackId = stack.StackId;

        var confirmation = AnsiConsole.Prompt(new ConfirmationPrompt("Are you sure?"));
        if (confirmation == true)
        {
            flashcardController.DeleteFlashcard(card);
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Flashcard not deleted![/]");
            GetFlashcardMenu(stack);
        }
        Console.ReadKey();
    }

    public void ViewAllFlashcards(StackDTO stack)
    {
        FlashcardsController flashcardController = new FlashcardsController();
        var cards = flashcardController.ViewAllFlashcards(stack);
        if (cards.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No flashcards found![/]");
            Console.ReadKey();
            return;
        }
        else
        {
            var table = new Table();
            table.Title = new TableTitle("All Flashcards", Style.Parse("bold yellow"));
            table.AddColumn("[bold]Id[/]");
            table.AddColumn("[bold]Question[/]");
            table.AddColumn("[bold]Answer[/]");

            int fakeId = 1;
            foreach (var card in cards)
            {
                table.AddRow(
                       fakeId.ToString(),
                       card.Question,
                       card.Answer
                   );
                fakeId++;
            }
            Console.Clear();
            AnsiConsole.Write(table);
        }

    }
}