namespace Flashcards.Models;

public class UpdateFlashcardDto
{
    public int Id { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
    public UpdateFlashcardDto(string front, string back, int id)
    {
        Id = id;
        Front = front;
        Back = back;
    }
}