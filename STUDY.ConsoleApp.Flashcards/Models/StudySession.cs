namespace STUDY.ConsoleApp.Flashcards.Models;

public class StudySession
{
    public int Id { get; init; } 
    
    public DateTime SessionDate { get; init; }

    public int Score { get; set; }
    
    public int StackId { get; init; }

    public StudySession() { }

    public StudySession(int stackId)
    {
        StackId = stackId;
    }
}