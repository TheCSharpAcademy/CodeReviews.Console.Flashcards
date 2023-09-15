namespace Flashcards;

class StudySession
{
    public long Id { get; set; }
    public long StackId { get; set; }
    public DateTime CompletedAt { get; set; }
    public int Score { get; set; }

    public StudySession(long id, long stackId, DateTime completedAt, int score)
    {
        Id = id;
        StackId = stackId;
        CompletedAt = completedAt;
        Score = score;
    }

    public StudySession(long stackId, DateTime completedAt, int score)
    {
        StackId = stackId;
        CompletedAt = completedAt;
        Score = score;
    }
}