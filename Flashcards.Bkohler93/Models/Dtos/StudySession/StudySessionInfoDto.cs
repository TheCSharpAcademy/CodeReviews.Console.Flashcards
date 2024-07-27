namespace Models;

public class StudySessionInfoDto
{
    public int Id { get; set; }
    public DateTime StudyTime { get; set; }
    public int Score { get; set; }
    public required string StackName { get; set; }
}
