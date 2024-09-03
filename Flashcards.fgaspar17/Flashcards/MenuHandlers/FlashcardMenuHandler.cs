using Flashcards.MenuEnums;
using FlashcardsLibrary;
using Spectre.Console;

namespace Flashcards;
public class FlashcardMenuHandler
{
    public Stack SelectedStack { get; set; }

    public void DisplayMenu()
    {
        StackMenuHandler.ShowStacks();
        string stackName = StackMenuHandler.GetStackNameFromUser(new ExistingModelValidator<string, Stack>
        { errorMsg = "Stack Name must exist", GetModel = StackController.GetStackByName });
        if (CancelSetup.IsCancelled(stackName)) return;

        SelectedStack = StackController.GetStackByName(stackName);

        MenuPresentation.MenuDisplayer<FlashcardMenuOptions>(() => $"[blue]Flashcard Menu - Current Stack: {SelectedStack.Name}[/]", HandleMenuOptions);
    }

    private bool HandleMenuOptions(FlashcardMenuOptions option)
    {
        switch (option)
        {
            case FlashcardMenuOptions.Back:
                return false;
            case FlashcardMenuOptions.ChangeStack:
                ChangeStackHandler();
                break;
            case FlashcardMenuOptions.ViewAll:
                ShowAllFlashcardsHandler();
                break;
            case FlashcardMenuOptions.ViewX:
                ShowXFlashcardsHandler();
                break;
            case FlashcardMenuOptions.CreateFlashcard:
                CreateFlashcardHandler();
                break;
            case FlashcardMenuOptions.UpdateFlashcard:
                UpdateFlashcardHandler();
                break;
            case FlashcardMenuOptions.DeleteFlashcard:
                DeleteFlashcardHandler();
                break;
            default:
                Console.WriteLine($"Option: {option} not valid");
                break;
        }
        return true;
    }

    private void ChangeStackHandler()
    {
        StackMenuHandler.ShowStacks();
        string name = StackMenuHandler.GetStackNameFromUser(new ExistingModelValidator<string, Stack>() { GetModel = StackController.GetStackByName });
        if (CancelSetup.IsCancelled(name)) return;

        Stack stackSelected = StackController.GetStackByName(name);
        if (stackSelected != null)
        {
            SelectedStack = stackSelected;
        }
    }

    private void CreateFlashcardHandler()
    {
        MenuPresentation.PresentMenu("[blue]Inserting[/]");

        string question = GetQuestionFromUser();
        if (CancelSetup.IsCancelled(question)) return;
        AnsiConsole.WriteLine();
        string answer = GetAnswerFromUser();
        if (CancelSetup.IsCancelled(answer)) return;

        FlashcardController.InsertFlashcard(new Flashcard { StackId = SelectedStack.StackId, Question = question, Answer = answer });
    }

    private void UpdateFlashcardHandler()
    {
        MenuPresentation.PresentMenu("[yellow]Updating[/]");
        ShowFlashcards();

        string flashcardPresentationId = GetFlashcardOrderIdFromUser(new ExistingModelValidator<int, Flashcard> { errorMsg = "Flashcard Id must exist",
            GetModel = GetFlashcardIdByOrderId });
        if (CancelSetup.IsCancelled(flashcardPresentationId)) return;
        int flashcarOrderId = Convert.ToInt32(flashcardPresentationId);

        AnsiConsole.WriteLine();
        string question = GetQuestionFromUser();
        if (CancelSetup.IsCancelled(question)) return;
        AnsiConsole.WriteLine();
        string answer = GetAnswerFromUser();
        if (CancelSetup.IsCancelled(answer)) return;

        FlashcardController.UpdateFlashcard(new Flashcard { FlashcardId = FlashcardController.GetFlashcardIdByOrderId(SelectedStack.StackId, flashcarOrderId)!.Value,
            StackId = SelectedStack.StackId, Question = question, Answer = answer });
    }

    private void DeleteFlashcardHandler()
    {
        MenuPresentation.PresentMenu("[red]Deleting[/]");
        ShowFlashcards();

        string flashcardPresentationId = GetFlashcardOrderIdFromUser(new ExistingModelValidator<int, Flashcard>
        {
            errorMsg = "Flashcard Id must exist",
            GetModel = GetFlashcardIdByOrderId
        });
        if (CancelSetup.IsCancelled(flashcardPresentationId)) return;
        int flashcarOrderId = Convert.ToInt32(flashcardPresentationId);

        FlashcardController.DeleteFlashcard(new Flashcard { FlashcardId = FlashcardController.GetFlashcardIdByOrderId(SelectedStack.StackId, flashcarOrderId)!.Value,
            Question = string.Empty, Answer = string.Empty });
    }

    private void ShowAllFlashcardsHandler()
    {
        MenuPresentation.PresentMenu("[green]Showing All Flashcards[/]");

        ShowFlashcards();

        Prompter.PressKeyToContinuePrompt();
    }

    private void ShowXFlashcardsHandler()
    {
        string message = "Introduce a number";

        string flascardNumber = Prompter.ValidatedTextPrompt(message, validations: new IntValidator());
        if (CancelSetup.IsCancelled(flascardNumber)) return;

        MenuPresentation.PresentMenu($"[green]Showing {flascardNumber} Flashcards[/]");

        ShowFlashcards(int.Parse(flascardNumber));

        Prompter.PressKeyToContinuePrompt();
    }

    private string GetFlashcardOrderIdFromUser(params IValidator[] validators)
    {
        string message = "Introduce an existing Id";

        return Prompter.ValidatedTextPrompt(message, validations: validators);
    }

    private string GetQuestionFromUser(params IValidator[] validators)
    {
        string message = "Introduce a Question";

        return Prompter.ValidatedTextPrompt(message, validations: validators);
    }

    public static string GetAnswerFromUser(params IValidator[] validators)
    {
        string message = "Introduce an Answer";

        return Prompter.ValidatedTextPrompt(message, validations: validators);
    }

    private void ShowFlashcards(int? number = null)
    {
        var flashcards = FlashcardService.GetFlashcards(SelectedStack.StackId);

        if (number != null && flashcards.Count > 0)
        {
            flashcards = flashcards.Take(number.Value).ToList();
        }

        OutputRenderer.ShowTable(flashcards, title: "Flashcards");
    }

    private Flashcard? GetFlashcardIdByOrderId(int orderId)
    {
        int? realId = FlashcardController.GetFlashcardIdByOrderId(SelectedStack.StackId, orderId);
        return realId.HasValue ? FlashcardController.GetFlashcardById(realId.Value) : null;
    }
}