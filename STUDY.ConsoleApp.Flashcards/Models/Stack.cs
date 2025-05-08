using STUDY.ConsoleApp.Flashcards.Models.DTOs;

namespace STUDY.ConsoleApp.Flashcards.Models;

public class Stack
{
    public int Id { get; init; }
    
    public required string Name { get; set; }

    public List<FlashcardDto>? Flashcards { get; set; }
    
    public List<StudySession>? StudySessions { get; set; }

    public override string ToString()
    {
        return Name;
    }
}