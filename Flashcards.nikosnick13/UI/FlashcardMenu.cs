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
                    PocessecViewOneFlashcards();

                    break;
                case FlashcardsMenuOptions.EditFlashcard:
                    AnsiConsole.MarkupLine("[yellow]Study mode not implemented yet![/]");
                    ReadKey();
                    break;
                case FlashcardsMenuOptions.DeleteFlashcard:
                    AnsiConsole.MarkupLine("[yellow]Study mode not implemented yet![/]");
                    ReadKey();
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

    private void PocessecViewOneFlashcards()
    {

        _flashcardController.ViewAllFlashcards();


        AnsiConsole.WriteLine("\nPlease enter the ID of the stack you want to view (or 0 to return to the main menu).");
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

        
        var flashcardDTO = _flashcardController.GetById(id);

        TableVisualisation.DisplayOneFlashcard(new List<DetailFlashcardDTO> { flashcardDTO } );

    }
}


