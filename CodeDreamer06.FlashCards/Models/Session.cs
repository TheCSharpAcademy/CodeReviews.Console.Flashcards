using System;

namespace FlashStudy.Models
{
  public class Session
  {
    public Session(string[] properties) {
      SessionId = Convert.ToInt32(properties[0]);
      CreatedOn = properties[1];
      Score = Convert.ToInt32(properties[2]);
      StackId = Convert.ToInt32(properties[3]);
    }

    public Session(int score, int stackId) {
      Score = score;
      StackId = stackId;
    }

    public Session(int sessionId, string createdOn, int score, int stackId) {
      SessionId = sessionId;
      CreatedOn = createdOn;
      Score = score;
      StackId = stackId;
    }

    public int SessionId { get; set; }
    public string CreatedOn { get; set; }
    public int Score { get; set; }
    public int StackId { get; set; }
  }
}
