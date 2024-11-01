namespace FlashcardsProgram.Flashcards;

public class UpdateFlashcardDto
{
    public string Front { get; set; }
    public string Back { get; set; }

    public UpdateFlashcardDto(string front, string back)
    {
        Front = front;
        Back = back;
    }
}