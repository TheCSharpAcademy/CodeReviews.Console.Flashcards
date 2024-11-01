namespace Flashcards.Dejmenek.Models;

public class Flashcard
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
}

public class FlashcardDTO
{
    public int Id { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
}
