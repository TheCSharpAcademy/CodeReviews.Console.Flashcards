namespace Flashcards.Models;

public class FlashcardDTO
{
    public string Question { get; set; }
    public string Answer { get; set; }
    public CardStack Stack { get; set; }
}
