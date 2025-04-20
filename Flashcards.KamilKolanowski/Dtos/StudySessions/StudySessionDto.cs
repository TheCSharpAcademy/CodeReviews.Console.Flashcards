namespace Flashcards.KamilKolanowski.Dtos.StudySessions;

public class StudySessionDto
{
    public int StudySessionId { get; set; }
    public int StackId { get; set; }
    public string StackName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Score { get; set; }
}