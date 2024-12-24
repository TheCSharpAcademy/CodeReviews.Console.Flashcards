namespace Flashcards.Models;

class FlashcardsModel
{
    internal int FlashcardId { get; set; }
    internal string? Front { get; set; }
    internal string? Back { get; set; }
    internal int StackId { get; set; }
}