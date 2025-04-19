namespace Flashcards.KamilKolanowski.Models;

public class UpdateCardDto
{
    public int FlashcardId { get; set; }
    public int StackId { get; set; }
    public string FlashcardTitle { get; set; } = string.Empty;
    public string ColumnToUpdate { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
}