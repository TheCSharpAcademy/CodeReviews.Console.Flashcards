namespace Flashcards.Views;

public class StackView : BaseView
{
    public string CreateStack()
    {
        return AnsiConsole.Prompt(new TextPrompt<string>("What's the name of the stack:")
            .ValidationErrorMessage("[red]The name must be at least 1 character, and at most 50 characters[/]")
            .Validate(i => i.Trim().Length is <= 50 and >= 1));
    }

    public List<Flashcard> RemoveFlashcards(List<Flashcard> chosenStackFlashcards)
    {
        return AnsiConsole.Prompt(new MultiSelectionPrompt<Flashcard>
            {
                Required = false,
                Title = "Select the flashcards to delete"
            }
            .AddChoices(chosenStackFlashcards)
        );
    }
}