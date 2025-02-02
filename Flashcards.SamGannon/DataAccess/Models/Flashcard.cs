namespace DataAccess.Models;

public class Flashcard
{
    public int FlashcardId {  get; set; }
    public int StackId { get; set; }
    public string? Question { get; set; } = string.Empty;
    public string? Answer { get; set; } = string.Empty;
}
