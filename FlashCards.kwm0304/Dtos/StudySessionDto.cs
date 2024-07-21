namespace FlashCards.kwm0304.Dtos;

public class StudySessionDto
{
  public int Id { get; set; }
  public DateTime Date { get; set; }
  public int Score { get; set; }
  public string StackName { get; set; }
  public StudySessionDto(DateTime date, int score, string stackName)
  {
    Date = date;
    Score = score;
    StackName = stackName;
  }
}