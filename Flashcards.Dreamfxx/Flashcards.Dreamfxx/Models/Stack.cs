namespace Flashcards.Dreamfxx.Models;
public class Stack
{
    public int StackId { get; set; }
    public required string Name { get; set; }
    public required List<Flashcard> Flashcards { get; set; }
    public required List<Session> Sessions { get; set; }
}

