namespace Buutyful.FlashCards.Models;

public class StudySession
{
    public int Id { get; set; }
    public int DeckId { get; set; }
    public Deck Deck { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Score { get; set; }
}
public record StudySessionCreateDto
{
    public int Score { get; private set; }
    public DateTime CreatedAt { get; private set; }
    private StudySessionCreateDto() { }
    private StudySessionCreateDto(int score, DateTime create) =>
        (Score, CreatedAt) = (score, create);
    public static StudySessionCreateDto Create(int score) =>
        new(score, DateTime.Now);
    
}
public record StudySessionDisplayDto(int Score, DateTime CreatedAt)
{
    public static implicit operator StudySessionDisplayDto(StudySession session) =>
        new(session.Score, session.CreatedAt);
}