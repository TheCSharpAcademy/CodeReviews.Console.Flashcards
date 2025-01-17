namespace Flashcards.DreamFXX.Models;

public class StackOfCards
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<Card> Cards { get; set; } = new();
}