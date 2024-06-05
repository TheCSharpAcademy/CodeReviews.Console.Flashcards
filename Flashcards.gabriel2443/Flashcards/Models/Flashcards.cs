namespace Flashcards.Models;

internal class FlashCards
{
    public int FlashcardId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int StackId { get; set; }
}

internal class FlashCardsDto
{
    public int StackId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
}