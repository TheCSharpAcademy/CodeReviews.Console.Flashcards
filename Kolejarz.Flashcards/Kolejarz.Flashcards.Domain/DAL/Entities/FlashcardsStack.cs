namespace Kolejarz.Flashcards.Domain.DAL.Entities;

public class FlashcardsStack
{
    public Guid StackId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Flashcard> Flashcards { get; set; }
    public List<StudySession> Sessions { get; set; }
    public DateTime CreatedDate { get; set; }
}
