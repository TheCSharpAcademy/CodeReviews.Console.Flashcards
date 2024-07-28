using Flashcards.Models;

namespace Models;

public class PlayStack
{
    public int Id { get; set; }
    public required string Name { get; set; }
   public required ICollection<Flashcard> Flashcards { get; set; } 
}
