namespace Flashcard_Application.Models;

public class StudySession
{
    public int SessionId { get; set; }
    public int StackId { get; set; }
    public DateTime SessionStartTime { get; set; }
    public DateTime SessionEndTime { get; set; }
    public int SessionScore { get; set; }
}
