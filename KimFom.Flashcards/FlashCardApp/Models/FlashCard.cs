namespace FlashCardApp.Models;

public class FlashCard
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
}