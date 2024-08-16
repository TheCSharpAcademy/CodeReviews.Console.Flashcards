namespace Flashcards.Models;
public class Stack : BaseEntity {
    public string Name { get; set; }
    public ICollection<Flashcard> Flashcards { get; set; }
}

