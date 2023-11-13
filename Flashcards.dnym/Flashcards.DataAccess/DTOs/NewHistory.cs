namespace Flashcards.DataAccess.DTOs;

public class NewHistory
{
    public DateTime StartedAt { get; set; }
    public int StackId { get; set; }
    public List<NewStudyResult> Results { get; set; } = new();
}
