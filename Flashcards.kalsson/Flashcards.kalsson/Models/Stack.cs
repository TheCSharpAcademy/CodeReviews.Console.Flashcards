using System.ComponentModel.DataAnnotations;

namespace Flashcards.kalsson.Models;

public class Stack
{
    [Key]
    public int StackId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Flashcard> Flashcards { get; set; }
    public virtual ICollection<StudySession> StudySessions { get; set; }
}