namespace STUDY.ConsoleApp.Flashcards.Models;

public class Flashcard
{
    public int Id { get; set; }

    public required string Front { get; set; } 
    
    public required string Back { get; set; }
    
    public int StackId { get; set; }
}