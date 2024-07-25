namespace Models;

public class StudySession
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public DateTime StudyTime { get; set; }
    public int Score { get; set; }

    public required Stack Stack { get; set; }
}