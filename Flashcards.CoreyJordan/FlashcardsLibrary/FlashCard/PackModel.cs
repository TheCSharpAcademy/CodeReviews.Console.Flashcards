namespace FlashcardsLibrary.FlashCard;
public class PackModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<CardModel> Pack { get; set; }
}
