namespace Flashcards.DataAccess.DTOs;

public class ExistingFlashcard
{
    public int Id { get; set; }
    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;
}
