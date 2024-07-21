namespace FlashCards.kwm0304.Models;

public class StudySession
{
  public int StudySessionId { get; set; }
  public int StackId { get; set; }
  public Stack Stack { get; set; }
  public DateTime StudiedAt { get; set; }
  public int Score { get; set; }

  public StudySession(int score, int stackId)
  {
    StudiedAt = DateTime.Now;
    Score = score;
    StackId = stackId;
  }
}