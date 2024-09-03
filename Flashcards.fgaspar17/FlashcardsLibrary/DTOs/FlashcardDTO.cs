namespace FlashcardsLibrary;
public class FlashcardDTO
{
    public int Id { get; set; }
    public required string Question { get; set; }
    public required string Answer { get; set; }
}