namespace Flashcards.Models;

public class Flashcard
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public string Title { get; set; } = default!;
    public string Question { get; set; } = default!;
    public string Answer { get; set; } = default!;
    public int Position { get; set; }

    public override string ToString()
    {
        return Title;
    }
}