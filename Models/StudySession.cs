using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Flashcards.TwilightSaw.Models;

public class StudySession
{
    [Key]
    public int Id { get; set; }
    public string Date { get; set; }
    public int Score { get; set; }

    public int CardStackId { get; set; }

    public CardStack CardStack { get; set; }

    public StudySession(string date, int score, int cardStackId)
    {
        Date = date;
        Score = score;
        CardStackId = cardStackId;
    }
}