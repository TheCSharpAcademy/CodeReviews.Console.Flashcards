namespace Flashcards.Dreamfxx.Dtos;
public class FlashcardDto
{
    public int Id { get; set; }
    public int PresentationId { get; set; }
    public string? Question { get; set; }
    public string? Answer { get; set; }
}