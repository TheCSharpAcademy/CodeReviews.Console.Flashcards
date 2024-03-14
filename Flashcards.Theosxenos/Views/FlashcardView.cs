namespace Flashcards.Views;

public class FlashcardView : BaseView
{
    public void UpdateFlashcard(Flashcard flashcard)
    {
        flashcard.Title = AskInput("Pick a title:", flashcard.Title);
        flashcard.Question = AskInput("Pick a question:", flashcard.Question);
        flashcard.Answer = AskInput("Pick an answer:", flashcard.Answer);
    }

    public Flashcard CreateFlashcard()
    {
        var flashcard = new Flashcard();
        UpdateFlashcard(flashcard);
        return flashcard;
    }

    public void ShowTable(List<FlashcardDto> flashcardDtos)
    {
        var table = new Table();

        table.AddColumn("Flashcard");
        table.AddColumn("Stack");

        flashcardDtos.ForEach(f => table.AddRow(f.FlashcardTitle, f.StackName));

        AnsiConsole.Write(table);

        AnsiConsole.MarkupLine("[gray]Press any key to go back[/]");
        Console.ReadKey();
    }
}