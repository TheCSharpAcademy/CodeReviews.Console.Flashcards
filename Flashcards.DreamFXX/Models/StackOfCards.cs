namespace Flashcards.DreamFXX.Models;

public class StackofCards
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<Card> CardsList { get; set; }
}