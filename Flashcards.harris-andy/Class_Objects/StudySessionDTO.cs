namespace Flashcards.harris_andy;

public class StudySessionDTO
{
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public int Questions { get; set; }

    public StudySessionDTO(DateTime date, int score, int questions)
    {
        Date = date;
        Score = score;
        Questions = questions;
    }
}