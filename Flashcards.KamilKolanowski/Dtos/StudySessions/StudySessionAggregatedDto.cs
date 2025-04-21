namespace Flashcards.KamilKolanowski.Dtos.StudySessions;

public class StudySessionAggregatedDto
{
    public int StudySessionId { get; set; }
    public int StackId { get; set; }
    public string StackName { get; set; } = string.Empty;
    public int Year { get; set; }
    public double January { get; set; }
    public double February { get; set; }
    public double March { get; set; }
    public double April { get; set; }
    public double May { get; set; }
    public double June { get; set; }
    public double July { get; set; }
    public double August { get; set; }
    public double September { get; set; }
    public double October { get; set; }
    public double November { get; set; }
    public double December { get; set; }
}
