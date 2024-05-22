namespace Flashcards.kalsson.Models;

public class StudySession
{
    public int SessionId { get; set; }
    public int StackId { get; set; }  // Foreign key linking to Stacks
    public DateTime SessionDate { get; set; }
    public int Score { get; set; }
}