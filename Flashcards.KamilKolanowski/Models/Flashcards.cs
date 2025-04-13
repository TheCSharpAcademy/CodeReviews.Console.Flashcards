namespace Flashcards.KamilKolanowski.Data;

internal class Flashcards
{
    private int FlashcardId { get; set; }
    private int StackId { get; set; }
    private string FlashcardTitle { get; set; } = string.Empty;
    private string FlashcardContent { get; set; } = string.Empty;
    private DateTime DateCreated { get; set; }
}