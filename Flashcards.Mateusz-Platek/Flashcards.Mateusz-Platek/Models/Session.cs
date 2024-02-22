namespace Flashcards.Mateusz_Platek.Models;

public class Session
{
    public int sessionId { get; set; }
    public int stackId { get; set; }
    public int score { get; set; }
    public DateTime dateTime { get; set; }
    public int maxScore { get; set; }
    public string stackName { get; set; }

    public Session(int sessionId, int stackId, int score, DateTime dateTime, int maxScore, string stackName)
    {
        this.sessionId = sessionId;
        this.stackId = stackId;
        this.score = score;
        this.dateTime = dateTime;
        this.maxScore = maxScore;
        this.stackName = stackName;
    }
}