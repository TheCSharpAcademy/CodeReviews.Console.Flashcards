namespace FlashCards.Doc415.Models;

internal class Flashcard
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int StackId { get; set; }
}
