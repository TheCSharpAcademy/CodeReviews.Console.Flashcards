using Ohshie.FlashCards.CardsManager;

namespace Ohshie.FlashCards.StacksManager;

public class Deck
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<FlashCard>? FlashCards { get; set; }
}