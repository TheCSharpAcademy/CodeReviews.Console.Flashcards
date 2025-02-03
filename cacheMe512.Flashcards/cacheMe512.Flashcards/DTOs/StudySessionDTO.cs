
namespace cacheMe512.Flashcards.DTOs;

public class StudySessionDTO
{
    public int Id { get;}
    public string StackName { get; }
    public DateTime Date { get; }
    public int Score { get; }

    public StudySessionDTO(int id, string stackName, DateTime date, int score)
    {
        Id = id;
        StackName = stackName;
        Date = date;
        Score = score;
    }
}
