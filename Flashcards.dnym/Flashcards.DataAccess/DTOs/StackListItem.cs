namespace Flashcards.DataAccess.DTOs;

public class StackListItem
{
    public int Id { get; set; }
    public string ViewName { get; set; } = string.Empty;
    public int Cards { get; set; }
    public DateTime? LastStudied { get; set; }
}
