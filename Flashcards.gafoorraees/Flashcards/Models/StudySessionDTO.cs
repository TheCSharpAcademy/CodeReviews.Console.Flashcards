namespace Flashcards.Models;

public class StudySessionDTO
{
    public int ID { get; set; }
    public int StackID { get; set; }
    public string StackName { get; set; }
    public DateTime Date { get; set; }
    public int Score { get; set; }
}
