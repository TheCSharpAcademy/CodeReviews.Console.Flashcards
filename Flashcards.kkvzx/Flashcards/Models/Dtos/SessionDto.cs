namespace Flashcards.Models.Dtos;

public class SessionDto : BaseEntity
{
    public int StackId { get; init; }
    public DateTime OccurenceDate { get; init; }
    public int Score { get; init; }

    public SessionDto()
    {
    }

    public SessionDto(int stackId, int score, DateTime occurenceDate)
    {
        StackId = stackId;
        Score = score;
        OccurenceDate = occurenceDate;
    }
}