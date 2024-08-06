namespace Model;
public class Stack
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public ICollection<Flashcard>? Flashcards { get; set; }
    public ICollection<Study>? Studies { get; set; }
}
