namespace Flashcards.Models;

public class FlashcardInfoDto
{
    public int Id { get; set; }
    public required string Front { get; set; }
    public required string Back { get; set; }
}
