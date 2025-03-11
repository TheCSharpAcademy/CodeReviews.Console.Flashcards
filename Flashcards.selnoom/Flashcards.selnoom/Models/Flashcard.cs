namespace Flashcards.selnoom.Models;

internal class Flashcard
{
    public int FlashcardId { get; set; }
    public int StackId { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
}
