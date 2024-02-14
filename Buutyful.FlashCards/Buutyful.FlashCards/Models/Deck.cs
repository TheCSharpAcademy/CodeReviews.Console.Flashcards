namespace Buutyful.FlashCards.Models;

public class Deck
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Category { get; set; } = null!;
    public List<FlashCard> FlashCards { get; set; } = new();
}
public record DeckDisplayDto(string Name, string Category, List<FlashCard> list)
{
    public static implicit operator DeckDisplayDto(Deck deck) => 
        new(deck.Name, deck.Category, new List<FlashCard>(deck.FlashCards));
}
 public record DeckCreateDto
{
    public string Name { get; private set; }
    public string Category { get; set; }    
    private DeckCreateDto() { }
    private DeckCreateDto(string name, string category) =>
        (Name, Category) = (name, category);
    public static DeckCreateDto Create(string name, string category) => new(name, category);
}