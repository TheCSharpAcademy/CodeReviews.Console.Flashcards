namespace Flashcards.Models;

internal class Flashcard
{
    public required string Term { get; set; }
    public required string Definition { get; set; }
    public int Id { get; set; }
    public int StackId { get; set; }
}