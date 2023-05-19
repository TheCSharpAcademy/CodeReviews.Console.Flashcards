namespace FlashcardsLibrary.FlashCard;
public class CardModel
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int DeckId { get; set; }
}
