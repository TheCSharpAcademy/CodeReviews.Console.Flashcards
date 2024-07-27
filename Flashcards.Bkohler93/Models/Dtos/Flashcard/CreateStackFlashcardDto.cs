namespace Flashcards.Models;

public class CreateStackFlashcardDto
{
    public string Front { get; set; }
    public string Back { get; set; }
    public int StackId { get; set; }
    public CreateStackFlashcardDto(string front, string back, int stackId)
    {
        Front = front;
        Back = back;
        StackId = stackId;
    }
}