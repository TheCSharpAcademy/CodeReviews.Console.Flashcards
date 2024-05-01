namespace Flashcards.Models;

public class StudySession
{
    public int SessionID { get; set; }
    public int StackID { get; set; }
    public DateTime SessionDate { get; set; }
    public int Score { get; set; }

    public StudySession() { }

    public StudySession(int stackID, int score)
    {
        StackID = stackID;
        SessionDate = DateTime.UtcNow;
        Score = score;
    }
}
