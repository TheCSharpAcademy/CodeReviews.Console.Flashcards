namespace cacheMe512.Flashcards.DTOs;

public class StudySessionDto
{
    public int Id { get;}
    public string StackName { get; }
    public DateTime Date { get; }
    public int TotalQuestions { get; }
    public int Score { get; }

    public StudySessionDto(int id, string stackName, DateTime date, int totalQuestions, int score)
    {
        Id = id;
        StackName = stackName;
        Date = date;
        TotalQuestions = totalQuestions;
        Score = score;
    }
}
