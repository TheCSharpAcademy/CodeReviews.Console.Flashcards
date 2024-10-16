namespace Flashcards.AnaClos.Models;

public class FlashCard
{
    public int Id { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
    public int StackId { get; set; }
}