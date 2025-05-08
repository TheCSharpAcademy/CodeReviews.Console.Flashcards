namespace STUDY.ConsoleApp.Flashcards.Models.DTOs;

public class FlashcardDto
{
    public int ViewId { get; set; }
    
    public int RealId { get; set; }

    public required string Front { get; set; } 
    
    public required string Back { get; set; }
    
    public override string ToString()
    {
        return "Id: " + ViewId + " | Front: " + Front + " | Back: " + Back;
    }
}