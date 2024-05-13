namespace FlashcardsLibrary.Models;
public class StudySession
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public DateTime Date { get; set; }
    public int Score { get; set; }

    public Stack Stack { get; set; }
}
