namespace Flashcards.Dreamfxx.Models;
public class Flashcard
{
    public int FlashcardId { get; set; }
    public required string Question { get; set; }
    public required string Answer { get; set; }
    public int StackId { get; set; }
    public Stack? Stack { get; set; }
}
