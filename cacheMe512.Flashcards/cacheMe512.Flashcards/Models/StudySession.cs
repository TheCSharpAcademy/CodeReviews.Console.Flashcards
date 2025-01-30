namespace cacheMe512.Flashcards.Models;

internal class StudySession
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public DateTime Date { get; set; }
    public int Score { get; set; }

    public static StudySession Start(int stackId)
    {
        return new StudySession
        {
            StackId = stackId,
            Date = DateTime.Now
        };
    }

    public void Complete(int score)
    {
        Score = score;
    }
}
