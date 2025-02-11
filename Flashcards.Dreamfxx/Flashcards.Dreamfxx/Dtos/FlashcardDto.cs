namespace Flashcards.Dreamfxx.Dtos;
public class FlashcardDto
{
    public int Id { get; set; }
    public required string Question { get; set; }
    public required string Answer { get; set; }
}
