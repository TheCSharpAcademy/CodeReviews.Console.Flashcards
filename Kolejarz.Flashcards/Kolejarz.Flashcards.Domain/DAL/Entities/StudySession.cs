namespace Kolejarz.Flashcards.Domain.DAL.Entities;

public class StudySession
{
    public Guid StudySessionId { get; set; }
    public Guid StackId { get; set; }
    public FlashcardsStack Stack { get; set; }
    public DateTime CreatedDate { get; set; }
    public int QuestionsAsked { get; set; }
    public int QuestionsAnswered { get; set; }
}
