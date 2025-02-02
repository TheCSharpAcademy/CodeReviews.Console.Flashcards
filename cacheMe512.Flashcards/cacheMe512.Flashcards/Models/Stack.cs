
namespace cacheMe512.Flashcards.Models;

internal class Stack
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public int Position { get; set; }
    public List<Flashcard> Flashcards { get; private set; } = new List<Flashcard>();

}
