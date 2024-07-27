namespace Flashcards.Models;

public class Stack
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<Flashcard> Flashcards { get; set; }
    public ICollection<StudySession> StudySessions { get; set; }

    public Stack()
    {
        Flashcards = new List<Flashcard>();
        StudySessions = new List<StudySession>();
    }
}
