
namespace cacheMe512.Flashcards.DTOs;

public class StudySessionDTO
{
    public string StackName { get; }
    public DateTime Date { get; }
    public int Score { get; }

    public StudySessionDTO(string stackName, DateTime date, int score)
    {
        StackName = stackName;
        Date = date;
        Score = score;
    }
}
