namespace Flashcards.KamilKolanowski.Models;

internal class Cards
{
    internal int FlashcardId { get; set; }
    internal int StackId { get; set; }
    internal string FlashcardTitle { get; set; } = string.Empty;
    internal string FlashcardContent { get; set; } = string.Empty;
    internal DateTime DateCreated { get; set; }
}