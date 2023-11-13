namespace Flashcards.DataAccess.DTOs;

public class NewFlashcard
{
    public int StackId { get; set; }
    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;
}
