namespace flashcards.Fennikko.Models;

public class Flashcards
{
    public int FlashcardId { get; set; }

    public int FlashcardIndex { get; set; }

    public required string CardFront { get; set; }

    public required string CardBack { get; set; }

    public int StackId { get; set; }
}