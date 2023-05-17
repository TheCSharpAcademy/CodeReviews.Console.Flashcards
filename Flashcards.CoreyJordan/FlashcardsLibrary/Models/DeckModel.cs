namespace FlashcardsLibrary.Models;
public class DeckModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<FlashCardModel> Deck { get; set; } = new();
}
