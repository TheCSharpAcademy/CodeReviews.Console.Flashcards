namespace Flashcards.DataAccess.DTOs;

public class HistoryListItem
{
    public int Id { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public string StackViewName { get; set; } = string.Empty;
    public int CardsStudied { get; set; }
    public int CorrectAnswers { get; set; }
}
