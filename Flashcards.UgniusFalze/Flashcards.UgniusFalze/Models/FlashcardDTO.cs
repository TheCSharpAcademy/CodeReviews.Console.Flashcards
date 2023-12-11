namespace Flashcards.UgniusFalze.Models;

public class FlashcardDTO
{
    private int FlashcardId {  get; set; }
    public int Order { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }

    public FlashcardDTO(int flashcardId, string front, string back, int order)
    {
        FlashcardId = flashcardId;
        Front = front;
        Back = back;
        Order = order;
    }

    public int GetActualId()
    {
        return FlashcardId;
    }
}