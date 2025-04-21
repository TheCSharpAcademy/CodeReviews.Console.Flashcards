namespace Flashcards.KamilKolanowski.Models;

internal class Cards
{
    public int FlashcardId { get; }
    public int StackId { get; set; }
    public string FlashcardTitle { get; set; } = string.Empty;
    public string FlashcardContent { get; set; } = string.Empty;
    public DateTime DateCreated { get; }
}
