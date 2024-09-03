namespace FlashcardsLibrary;

public class Flashcard
{
    public int OrderId { get; set; }
    public int FlashcardId { get; set; }
    public int StackId { get; set; }
    public required string Question { get; set; }
    public required string Answer { get; set; }
}