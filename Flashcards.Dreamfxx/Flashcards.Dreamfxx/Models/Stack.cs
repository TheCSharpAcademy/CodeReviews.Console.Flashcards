namespace Flashcards.Dreamfxx.Models;
public class Stack
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Flashcard> Cards { get; set; }
}
