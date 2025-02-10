using static System.Console;
using Flashcards.nikosnick13.Controllers;
using Spectre.Console;
using static Flashcards.nikosnick13.Enums.Enums;
using Flashcards.nikosnick13.DTOs;

namespace Flashcards.nikosnick13.UI;

internal class FlashcardMenu
{
    private readonly FlashcardController _flashcardController;
    private readonly StackController _stackController;

    public FlashcardMenu()
    {
        _flashcardController = new FlashcardController();
        _stackController = new StackController();
    }

    public void ShowFlashcartMenu()
    {
        bool isFlashCardMenuRunning = true;

        while (isFlashCardMenuRunning)
        {
            Clear();
            var flashcadMenu = AnsiConsole.Prompt(
                new SelectionPrompt<FlashcardsMenuOptions>()
                .Title("What would you like to do?")
                .AddChoices(
                    FlashcardsMenuOptions.AddFlashcards,
                    FlashcardsMenuOptions.ViewAllFlashcards,
                    FlashcardsMenuOptions.ViewFlashcard,
                    FlashcardsMenuOptions.EditFlashcard,
                    FlashcardsMenuOptions.DeleteFlashcard,
                    FlashcardsMenuOptions.ReturnToMainMenu
                    ));

            switch (flashcadMenu)
            {
                case FlashcardsMenuOptions.AddFlashcards:
                    ProcessAdd();
                    break;
                case FlashcardsMenuOptions.ViewAllFlashcards:
                    _flashcardController.ViewAllFlashcards();
                    break;
                case FlashcardsMenuOptions.ViewFlashcard:
                    ProcessViewOneFlashcards();
                    break;
                case FlashcardsMenuOptions.EditFlashcard:
                    ProcessEdit();
                    break;
                case FlashcardsMenuOptions.DeleteFlashcard:
                    ProcessDelete();
                    break;
                case FlashcardsMenuOptions.ReturnToMainMenu:
                    isFlashCardMenuRunning = false;
                    break;
            }


        }

    }

    public void ProcessAdd()
    {
        AnsiConsole.Clear();

        var stacks = _stackController.ViewAllStacks();

        int stackId = AnsiConsole.Prompt(new TextPrompt<int>("\nEnter the [green]ID[/] of the stack to add the flashcard:"));

        var selectedStack = stacks.FirstOrDefault(stack => stack.Id == stackId);

        if (selectedStack == null)
        {
            AnsiConsole.MarkupLine("\n[red]Stack not found![/]");
            return;
        }

        var QuestionInput = GetTheQuestion("\nEnter the question of the new Flashcard, or type '0' to return to the Flashcard Menu.\n");

        var AnswerInput = GetTheQuestion("\nEnter the answer of the new Flashcard, or type '0' to return to the Flashcard Menu.\n");

        var newFlashcard = new BasicFlashcardDTO
        {
            Question = QuestionInput,
            Answer = AnswerInput,
            StackId = selectedStack.Id

        };

        _flashcardController.InsertFlashcart(newFlashcard);

    }

    private string GetTheQuestion(string msg)
    {
        WriteLine(msg);

        string userInputQuestion = AnsiConsole.Prompt(new TextPrompt<string>("\nNew Question:"));

        if (userInputQuestion == "0") ShowFlashcartMenu();
        while (!Validation.isValidString(userInputQuestion))
        {
            AnsiConsole.MarkupLine("[red]Invalid input. Try again.[/]");
            userInputQuestion = AnsiConsole.Ask<string>(msg);
        }

        return userInputQuestion;

    }

    private string GetTheAnswer(string msg)
    {
        string userInputAnswer = AnsiConsole.Prompt(new TextPrompt<string>("\nNew Answer:"));

        if (userInputAnswer == "0") ShowFlashcartMenu();
        while (!Validation.isValidString(userInputAnswer))
        {
            AnsiConsole.MarkupLine("\n[red]Invalid input. Try again.[/]");
            userInputAnswer = AnsiConsole.Ask<string>(msg);
        }

        return userInputAnswer;
    }

    //NOT WORKING TODO: FIX 
    private void ProcessViewOneFlashcards()
    {

        _flashcardController.ViewAllFlashcards();


        AnsiConsole.WriteLine("\nPlease enter the ID of the flashcard you want to view (or 0 to return to the main menu).");
        string? userInput = ReadLine();

        if (userInput == "0")
        {
            ShowFlashcartMenu();
            return;
        }

        while (!Validation.isValidInt(userInput))
        {
            Console.WriteLine("Invalid input. Please enter a valid integer ID.");
            userInput = Console.ReadLine();
        }

        int id = Int32.Parse(userInput);


        var flashcardDTO = _flashcardController.ViewFlashcardById(id);

        TableVisualisation.DisplayOneFlashcard(new List<DetailFlashcardDTO> { flashcardDTO });
    
    }

  

    private void ProcessDelete()
    {


        _flashcardController.ViewAllFlashcards();

        AnsiConsole.WriteLine("\nPlease enter the ID of the category you want to delete (or 0 to return to the main menu).");
        string? userInputId = ReadLine();

        if (userInputId == "0")
        {
            ShowFlashcartMenu();
            return;
        }

        if (!Validation.isValidInt(userInputId))
        {
            AnsiConsole.MarkupLine("[red]Invalid input. Please enter a valid numeric ID.[/]");
        }

        int id = Int32.Parse(userInputId);

        var selectedFlashcard = _flashcardController.GetFlashcardById(id);

        _flashcardController.DeleteFlashcardById(selectedFlashcard.Id);

    }

    private void ProcessEdit()
    {
        while (true) 
        {
            _flashcardController.ViewAllFlashcards();


            AnsiConsole.WriteLine("\nPlease enter the ID of the category you want to edit (or 0 to return to the main menu).");
            string? userInputId = ReadLine();

            if (userInputId == "0")
            {
                ShowFlashcartMenu();
                return;
            }
            if (!Validation.isValidInt(userInputId))
            {
                AnsiConsole.MarkupLine("[red]Invalid input. Please enter a valid numeric ID.[/]");
            }

            int id = Int32.Parse(userInputId);

            var selectedFlashcard = _flashcardController.GetFlashcardById(id);

            if (selectedFlashcard == null)
            {
                AnsiConsole.WriteLine($"Record with ID {id} doesn't exist. Please try again.");
                ReadKey();
                continue;
            }

            string newQuestion = GetTheQuestion("Please insert the new question. Type 0 to return to the main menu");
            string newAnswer = GetTheAnswer("Please insert the new answer. Type 0 to return to the main menu");

            var stacks = _stackController.ViewAllStacks();

            int stackId = AnsiConsole.Prompt(new TextPrompt<int>("\nEnter the [green]ID[/] of the stack to add the flashcard:"));

            var newSelectedStack = stacks.FirstOrDefault(stack => stack.Id == stackId);

            if (newSelectedStack == null || !stacks.Any(s => s.Id == stackId))
            {
                AnsiConsole.MarkupLine("\n[red]Stack not found![/]");
                ReadKey();
                return;
            }

            var flashcardDto = new BasicFlashcardDTO
            {
                Id = selectedFlashcard.Id,
                Question = newQuestion,
                Answer = newAnswer,
                StackId = newSelectedStack.Id

            };

            _flashcardController.EditFlashcard(flashcardDto);

            AnsiConsole.MarkupLine("\n[green]Flashcard updated successfully![/]");
        }
       

    }
}


