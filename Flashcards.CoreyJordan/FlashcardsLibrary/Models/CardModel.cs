namespace FlashcardsLibrary.Models;
public class CardModel
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int DeckId { get; set; }
    public string DeckName { get; set; }

    public CardModel()
    {
        
    }
    public CardModel(int id, string q, string a, int deck)
    {
        Id = id;
        Question = q;
        Answer = a;
        DeckId = deck;
    }
}
