namespace Flashcards.Controllers;

public class FlashcardController
{
    private readonly FlashcardRepository repository = new();
    private readonly FlashcardView view = new();

    public void CreateFlashcard()
    {
        try
        {
            var stack = GetStackFromMenu("Choose a stack to place the card in:");

            var continueAdding = true;
            while (continueAdding)
            {
                var flashcard = view.CreateFlashcard();
                flashcard.StackId = stack.Id;
                flashcard.Position = repository.GetLastPositionByStackId(stack.Id);
                repository.CreateFlashcard(flashcard);
                view.ShowSuccess("Flashcard created successfully.");
                continueAdding = view.AskConfirm("Do you want to add another flashcard?");
            }
        }
        catch (NotFoundException e)
        {
            view.ShowError(e.Message);
        }
    }

    public void ListFlashcards()
    {
        try
        {
            var stack = GetStackFromMenu();
            var dtoList = stack.Flashcards.Select(f => new FlashcardDto(f.Title, stack.Name)).ToList();

            view.ShowTable(dtoList);
        }
        catch (NotFoundException e)
        {
            view.ShowError(e.Message);
        }
    }

    public void UpdateFlashcard()
    {
        var stack = GetStackFromMenu();
        var toUpdateFlashcard = view.ShowMenu(stack.Flashcards, "Choose a flashcard to update");
        view.UpdateFlashcard(toUpdateFlashcard);

        try
        {
            repository.UpdateFlashcard(toUpdateFlashcard);
        }
        catch (NotFoundException e)
        {
            view.ShowError(e.Message);
        }
    }

    private Stack GetStackFromMenu(string menuTitle = "Choose a stack to list the cards from")
    {
        var stacks = new Repository().GetAllStacks();
        if (stacks.Count == 0) throw new NoStacksFoundException();

        return view.ShowMenu(stacks, menuTitle);
    }
}