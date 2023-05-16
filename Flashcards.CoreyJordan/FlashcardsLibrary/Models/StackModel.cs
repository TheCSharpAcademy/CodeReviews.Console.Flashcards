namespace FlashcardsLibrary.Models;
public class StackModel
{
    public string Name { get; set; }
    public List<FlashCardModel> Stack { get; set; } = new();
}
