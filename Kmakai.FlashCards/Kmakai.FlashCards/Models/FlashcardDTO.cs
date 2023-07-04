namespace Kmakai.FlashCards.Models;

public class FlashcardDTO
{
    public int Id { get; set; }
    public string? Front { get; set; }
    public string? Back { get; set; }

    public FlashcardDTO(int id, string? front, string? back)
    {
        Id = id;
        Front = front;
        Back = back;
    }
}
