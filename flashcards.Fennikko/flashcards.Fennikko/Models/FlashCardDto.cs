namespace flashcards.Fennikko.Models;

public class FlashcardDto
{
    public int FlashcardIndex { get; set; }

    public required string CardFront { get; set; }

    public required string CardBack { get; set; }
}