namespace Ohshie.FlashCards.CardsManager;

public class FlashCard
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
    public int DeckId { get; set; }
}