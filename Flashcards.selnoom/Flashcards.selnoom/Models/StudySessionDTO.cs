namespace Flashcards.selnoom.Models;

class StudySessionDTO
{
    public int StudySessionId { get; set; }
    public string StackName { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public DateTime SessionDate { get; set; }
}
