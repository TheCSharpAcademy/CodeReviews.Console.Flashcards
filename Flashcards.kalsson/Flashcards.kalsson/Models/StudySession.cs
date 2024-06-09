namespace Flashcards.kalsson.Models;

public class StudySession
{
    public int SessionId { get; set; }
    public int StackId { get; set; }
    public DateTime StudyDate { get; set; }
    public int Score { get; set; }
    public virtual Stack Stack { get; set; }
}