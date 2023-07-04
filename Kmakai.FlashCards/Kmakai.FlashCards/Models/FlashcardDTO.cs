namespace Kmakai.FlashCards.Models;

public class FlashcardDto
{
    public int Id { get; set; }
    public string? Front { get; set; }
    public string? Back { get; set; }

    public FlashcardDto(int id, string? front, string? back)
    {
        Id = id;
        Front = front;
        Back = back;
    }
}
