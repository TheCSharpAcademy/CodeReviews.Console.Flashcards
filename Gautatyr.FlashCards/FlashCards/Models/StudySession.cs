namespace FlashCards.Models;

public class StudySession
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string score { get; set; }
    public int StackId { get; set; }
}

public class StudySessionDTO
{
    public DateTime Date { get; set; }
    public string Score { get; set; }
    public string StackTheme { get; set; }
}
