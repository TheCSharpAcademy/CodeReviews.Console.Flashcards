namespace Flashcards.yemiOdetola.Models;

public class Session
{
  public int id { get; set; }
  public int stackId { get; set; }
  public int score { get; set; }
  public DateTime dateTime { get; set; }
  public int maxScore { get; set; }
  public string stackName { get; set; }

  public Session(int id, int stackId, int score, DateTime dateTime, int maxScore, string stackName)
  {
    this.id = id;
    this.stackId = stackId;
    this.score = score;
    this.dateTime = dateTime;
    this.stackName = stackName;
    this.maxScore = maxScore;
  }
}