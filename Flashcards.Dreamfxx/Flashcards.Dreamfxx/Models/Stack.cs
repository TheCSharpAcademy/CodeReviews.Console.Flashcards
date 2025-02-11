namespace Flashcards.Dreamfxx.Models;
public class Stack
{
    public int StackId { get; set; }
    public required string Name { get; set; }
    public List<Flashcard> Flashcards { get; set; };
    public List<Session> Sessions { get; set; }
}

