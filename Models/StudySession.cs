using System.ComponentModel.DataAnnotations;

namespace Flashcards.TwilightSaw.Models;

public class StudySession
{
    [Key]
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int Score { get; set; }
    public int CardStackId { get; set; }
    public CardStack CardStack { get; set; }
    public StudySession(DateOnly date, int score, int cardStackId)
    {
        Date = date;
        Score = score;
        CardStackId = cardStackId;
    }
}