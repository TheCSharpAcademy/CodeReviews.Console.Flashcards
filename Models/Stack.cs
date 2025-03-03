using System.ComponentModel.DataAnnotations;

namespace dotnetMAUI.Flashcards.Models;
public class Stack
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Flashcard> Flashcards { get; set; } = new();
    public List<StudySession> StudySessions { get; set; } = new();
}
