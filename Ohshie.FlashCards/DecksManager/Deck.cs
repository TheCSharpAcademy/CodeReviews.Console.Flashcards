using Ohshie.FlashCards.CardsManager;

namespace Ohshie.FlashCards.StacksManager;

public class Deck
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<FlashCard>? FlashCards { get; set; }
}

public class DeckDto
{
    public int Id { get; set; }
    public int ViewId { get; set; }
    public string? DeckName { get; set; }
    public string? DeckDescription { get; set; }
    public int AmountOfFlashcards { get; set; }
}