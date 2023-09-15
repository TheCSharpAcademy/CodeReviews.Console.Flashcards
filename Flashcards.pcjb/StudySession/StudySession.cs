namespace Flashcards;

class StudySession
{
    public long Id { get; set; }
    public long StackId { get; set; }
    public DateTime CompletedAt { get; set; }
    public int ScorePercent { get; set; }

    public StudySession(long id, long stackId, DateTime completedAt, int scorePercent)
    {
        Id = id;
        StackId = stackId;
        CompletedAt = completedAt;
        ScorePercent = scorePercent;
    }

    public StudySession(long stackId, DateTime completedAt, int scorePercent)
    {
        StackId = stackId;
        CompletedAt = completedAt;
        ScorePercent = scorePercent;
    }
}