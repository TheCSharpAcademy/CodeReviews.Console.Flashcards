namespace Flashcards.DataAccess.DTOs;

public class ExistingStudyResult
{
    public int Ordinal { get; set; }
    public string Front { get; set; } = "";
    public DateTime AnsweredAt { get; set; }
    public bool WasCorrect { get; set; }
}
