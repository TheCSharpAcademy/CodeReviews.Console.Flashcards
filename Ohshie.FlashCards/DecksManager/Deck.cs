using Ohshie.FlashCards.CardsManager;

namespace Ohshie.FlashCards.StacksManager;

public class Deck
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<FlashCard>? FlashCards { get; set; }
}

// ok, real talk - using DTO here feels extremely stupid, as i basically use almost the same props
// as a real Deck. But w/e i guess, i'll try to imagine that app will grow 100 times and DTO would be useful
public class DeckDto
{
    public int Id { get; set; }
    public int ViewId { get; set; }
    public string? DeckName { get; set; }
    public string? DeckDescription { get; set; }
    public int AmountOfFlashcards { get; set; }
}