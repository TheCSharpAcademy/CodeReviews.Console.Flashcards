namespace Models;

public class CreateStudySessionDto
{
    public int Id { get; set; }
    public DateTime StudyTime { get; set; }
    public int Score { get; set; }
    public int StackId { get; set; }
}
