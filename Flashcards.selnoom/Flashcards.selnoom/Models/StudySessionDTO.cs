namespace Flashcards.selnoom.Models;

internal class StudySessionDTO
{
    public int StudySessionId { get; set; }
    public string StackName { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public DateTime SessionDate { get; set; }
}
