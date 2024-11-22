using FlashCards.Controllers;
using FlashCards.Data;
using FlashCards.Utilities;
using FlashCards.Views;
using Spectre.Console;

namespace FlashCards.Services;
public static class FlashcardService
{
    private static FlashcardController _flashcardController = new FlashcardController(new DatabaseService());
    internal static string ChangeCurrentStack() => StackExtensions.ChooseStack();
    
    internal static void ViewAllFlashcards(string currentStack)
    {
        int stack_id = FlashcardExtensions.GetStack_id(currentStack);
        List<FlashCardDto> flashcards = _flashcardController.GetAllFlashcards(stack_id);
        List<FlashCardDto> flashcardsInOrder = FlashcardExtensions.GetAllFlashcardsInOrder(flashcards);
        TableVisualisation.ShowFlashcards(flashcardsInOrder);
    }

    internal static void ViewSpecificAmount(string currentStack)
    {
        int stack_id = FlashcardExtensions.GetStack_id(currentStack);
        string numberOfFlashcards = FlashcardExtensions.GetNumberOfFlashCards();
        List<FlashCardDto> flashcards = _flashcardController.GetAmountOfFlashcards(numberOfFlashcards, stack_id);
        List<FlashCardDto> flashcardsInOrder = FlashcardExtensions.GetAllFlashcardsInOrder(flashcards);
        TableVisualisation.ShowFlashcards(flashcardsInOrder);
    }

    internal static void CreateFlashcard(string currentStack)
    {
        string flashcardQuestion = FlashcardExtensions.GetFlashcardInfo("front");
        string flashcardAnswer = FlashcardExtensions.GetFlashcardInfo("back");
        int stack_id = FlashcardExtensions.GetStack_id(currentStack);
        _flashcardController.InsertFlashcard(flashcardQuestion, flashcardAnswer, stack_id);
        AnsiConsole.MarkupLine("[green]Flashcard created![/]");
    }

    internal static void EditFlashcard(string currentStack)
    {
        (int flashcardId, string flashcardFront, string flashcardBack) = FlashcardExtensions.GetFlashcard(currentStack);

        string userInput = AnsiConsole.Prompt(
            new TextPrompt<string>("What would you like to update: ([yellow]front, back, or both[/])")
                .ValidationErrorMessage("[red]Invalid input! please enter 'front', 'back' or 'both'[/]")
                .Validate(input =>
                {
                    input.ToLower();
                    return input == "front" || input == "back" || input == "both"
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Input must be 'front', 'back', 'both'[/].");
                }));
        AnsiConsole.MarkupLine($"You selected: [yellow]{userInput}[/]");

        switch (userInput.ToLower())
        {
            case "front":
                UpdateFlashcard(currentStack, flashcardFront: flashcardFront, updateFront: true);
                break;
            case "back":
                UpdateFlashcard(currentStack, flashcardBack: flashcardBack, updateBack: true);
                break;
            case "both":
                UpdateFlashcard(currentStack, flashcardFront, flashcardBack, updateBack: true, updateFront: true);
                break;
        }
    }

    internal static void DeleteFlashcard(string currentStack)
    {
        (int flashcardId, string flashcardFront, _) = FlashcardExtensions.GetFlashcard(currentStack);
        int stack_id = FlashcardExtensions.GetStack_id(currentStack);
        _flashcardController.DeleteFlashcard(flashcardFront, stack_id);
        AnsiConsole.MarkupLine($"[green]Flashcard deleted with ID:[/] [cyan]{flashcardId}[/]");
        Util.AskUserToContinue();
    }

    internal static void DeleteAllFlashcards(string currentStack)
    {
        int stack_id = FlashcardExtensions.GetStack_id(currentStack);
        _flashcardController.DeleteAllFlashcards(stack_id);
    }


    private static void UpdateFlashcard(string currentStack, string flashcardFront = "", string flashcardBack = "",
        bool updateFront = false,
        bool updateBack = false)
    {
        if (updateFront)
        {
            string frontOfCard = FlashcardExtensions.GetFlashcardInfo("front");
            _flashcardController.UpdateFrontOfFlashcard(flashcardFront, frontOfCard,
                FlashcardExtensions.GetStack_id(currentStack));
            AnsiConsole.MarkupLine($"[green]Flashcard back updated to[/] [cyan]{frontOfCard}[/]");
        }

        if (updateBack)
        {
            string backOfCard = FlashcardExtensions.GetFlashcardInfo("back");
            _flashcardController.UpdateBackOfFlashcard(flashcardBack, backOfCard,
                FlashcardExtensions.GetStack_id(currentStack));
            AnsiConsole.MarkupLine($"[green]Flashcard back updated to[/] [cyan]{backOfCard}[/]");
        }

        Util.AskUserToContinue();
    }

    internal static List<FlashCardDto> GetAllFlashcards(string currentStack)
    {
        return _flashcardController.GetAllFlashcards(FlashcardExtensions.GetStack_id(currentStack));
    }
}