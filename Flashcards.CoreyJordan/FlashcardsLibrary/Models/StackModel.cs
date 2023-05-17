namespace FlashcardsLibrary.Models;
public class StackModel
{
    public string Name { get; set; }
    public List<FlashCardModel> Deck { get; set; } = new();
}
