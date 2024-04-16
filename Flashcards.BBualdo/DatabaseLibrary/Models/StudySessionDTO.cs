namespace DatabaseLibrary.Models;

public class StudySessionDTO
{
  public int Id { get; set; }
  public DateTime Date { get; set; }
  public int Score { get; set; }
  public string StackName { get; set; }

  public StudySessionDTO(int id, DateTime date, int score, string stackName)
  {
    Id = id;
    Date = date;
    Score = score;
    StackName = stackName;
  }
}