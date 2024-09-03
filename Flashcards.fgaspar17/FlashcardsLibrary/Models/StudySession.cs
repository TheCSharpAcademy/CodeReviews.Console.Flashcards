namespace FlashcardsLibrary;

public class StudySession
{
    public int SessionId { get; set; }
    public int StackId { get; set; }
    public string? StackName { get; set; }
    public DateTime SessionDate { get; set; }
    public int Score { get; set; }
}