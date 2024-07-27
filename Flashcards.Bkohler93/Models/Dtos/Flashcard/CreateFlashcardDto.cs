namespace Flashcards.Models;

public class CreateFlashcardDto
{
    public string Front { get; set; }
    public string Back { get; set; }

    public CreateFlashcardDto(string front, string back)
    {
        Front = front;
        Back = back;
    }
}