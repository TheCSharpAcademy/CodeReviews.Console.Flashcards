namespace FlashCards;

public class StudySession
{
    public int Id { get; set; }
    public string Date { get; set; } = "";
    public int Correct { get; set; }
    public int Total { get; set; }
    public int StackId { get; set; }
}