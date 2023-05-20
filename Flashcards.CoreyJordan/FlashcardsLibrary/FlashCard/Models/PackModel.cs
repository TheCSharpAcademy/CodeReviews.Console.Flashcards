namespace FlashcardsLibrary.FlashCard.Models;
public class PackModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<CardModel> Pack { get; set; } = new();

    public PackModel(string name)
    {
        Name = name;
    }
}
