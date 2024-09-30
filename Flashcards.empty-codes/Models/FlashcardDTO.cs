namespace Flashcards.empty_codes.Models;

internal class FlashcardDto
{
    public int FlashcardId {  get; set; }
    public string? Question { get; set; }
    public string? Answer { get; set; }
    public int StackId { get; set; }
}