namespace Flashcards.Models;

public class Flashcard
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;

    public int StackId { get; set; }
    public Stack Stack { get; set; }
}