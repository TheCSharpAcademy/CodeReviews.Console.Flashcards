namespace FlashcardsProgram.Flashcards;

public class CreateFlashcardDTO
{
    public string Front { get; set; }
    public string Back { get; set; }
    public int StackId { get; set; }

    public CreateFlashcardDTO(string front, string back, int stackId)
    {
        Front = front;
        Back = back;
        StackId = stackId;
    }
}