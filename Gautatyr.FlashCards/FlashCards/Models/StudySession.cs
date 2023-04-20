namespace FlashCards.Models;

public class StudySession
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Score { get; set; }
    public int StackId { get; set; }
}

public class StudySessionDto
{
    public DateTime Date { get; set; }
    public string Score { get; set; }
    public string StackTheme { get; set; }
}
