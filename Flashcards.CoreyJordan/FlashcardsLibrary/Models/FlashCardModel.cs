namespace FlashcardsLibrary.Models;
public class FlashCardModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;
    public int DeckId { get; set; }
}
