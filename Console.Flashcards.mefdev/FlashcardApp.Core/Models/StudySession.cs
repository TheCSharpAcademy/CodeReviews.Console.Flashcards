namespace FlashcardApp.Core.Models;

public class StudySession
{
    public int StudySessionId { get; set; }
    public int StackId { get; set; }
    public DateTime Date {get; set; }
    public int Score {get; set; }

   protected readonly StudySession(){

    }

}