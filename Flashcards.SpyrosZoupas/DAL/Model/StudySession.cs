namespace Flashcards.DAL.Model;

public class StudySession
{
    public int ID { get; set; }
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public int StackID { get; set; }
}
