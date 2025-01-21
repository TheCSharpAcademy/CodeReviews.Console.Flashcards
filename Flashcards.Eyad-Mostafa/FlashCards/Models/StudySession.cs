namespace FlashCards.Models;

internal class StudySession
{
    public int StudySessionId { get; set; }
    public int StackId { get; set; }
    public DateTime SessionDate { get; set; }
    public int Score { get; set; }
}
