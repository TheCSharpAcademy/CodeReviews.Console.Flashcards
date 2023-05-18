namespace FlashcardsLibrary.Models;
public class FlashCardModel
{
    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;
    public int DeckId { get; set; }
}
