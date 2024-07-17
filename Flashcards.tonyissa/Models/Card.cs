namespace Flashcards.Models;

public class Card
{
    public int Id { get; set; }
    public string Front { get; set; } = String.Empty;
    public string Back { get; set; } = String.Empty;
    public int StackId { get; set; }

    public Card()
    {

    }
}