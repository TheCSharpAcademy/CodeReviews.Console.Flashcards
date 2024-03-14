namespace Flashcards.Models;

public class Stack
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public List<Flashcard> Flashcards { get; set; } = [];

    public override string ToString()
    {
        return Name;
    }
}