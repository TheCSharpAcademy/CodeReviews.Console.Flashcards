namespace Kolejarz.Flashcards.Domain.DAL.Entities;

public class Flashcard
{
    public Guid FlashcardId { get; set; }
    public string FrontSide { get; set; }
    public string BackSide { get; set; }
    public Guid StackId { get; set; }
    public FlashcardsStack Stack { get; set; }
    public DateTime CreatedDate { get; set; }
}
