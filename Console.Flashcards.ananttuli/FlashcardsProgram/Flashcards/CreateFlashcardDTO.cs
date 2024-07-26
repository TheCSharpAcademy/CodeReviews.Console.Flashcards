namespace FlashcardsProgram.Flashcards;

public class CreateFlashcardDto
{
    public string Front { get; set; }
    public string Back { get; set; }
    public int StackId { get; set; }

    public CreateFlashcardDto(string front, string back, int stackId)
    {
        Front = front;
        Back = back;
        StackId = stackId;
    }
}