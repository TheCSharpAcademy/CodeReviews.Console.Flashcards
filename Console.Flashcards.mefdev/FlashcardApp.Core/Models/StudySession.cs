namespace FlashcardApp.Core.Models;

public class StudySession
{
    public int Id { get; set; }
    public Stack stack { get; set; }
    public DateTime CurrentDate {get; set; }
    public int Score {get; set; }

    public StudySession()
    {

    }
}