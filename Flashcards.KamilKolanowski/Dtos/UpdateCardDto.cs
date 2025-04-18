namespace Flashcards.KamilKolanowski.Models;

public class UpdateCardDto
{
    public int StackId { get; set; }
    public string FlashcardTitle { get; set; }
    public string ColumnToUpdate { get; set; }
    public string NewValue { get; set; }
}