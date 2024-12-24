namespace Flashcards.Models;

class FlashcardsDTO
{
    internal int DisplayId { get; set; }
    internal string? Front { get; set; }
    internal string? Back { get; set; }
    internal string? StackName { get; set; }
}