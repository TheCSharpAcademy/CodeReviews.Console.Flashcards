namespace FlashcardsLibrary.Models;
public class Stack
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<StudySession> StudySession { get; set; }
}
