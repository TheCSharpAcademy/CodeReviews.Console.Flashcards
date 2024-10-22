namespace Flashcards.harris_andy;

public class StudySessionRecord
{
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public int Questions { get; set; }
    public int StackID { get; set; }

    public StudySessionRecord(DateTime date, int score, int questions, int stackID)
    {
        Date = date;
        Score = score;
        Questions = questions;
        StackID = stackID;
    }
}