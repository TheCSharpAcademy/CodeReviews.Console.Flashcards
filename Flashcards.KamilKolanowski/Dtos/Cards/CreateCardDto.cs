namespace Flashcards.KamilKolanowski.Models;

public class CreateCardDto
{
    public int StackId { get; set; }
    public string FlashcardTitle { get; set; } = String.Empty;
    public string FlashcardContent { get; set; } = String.Empty;
}
