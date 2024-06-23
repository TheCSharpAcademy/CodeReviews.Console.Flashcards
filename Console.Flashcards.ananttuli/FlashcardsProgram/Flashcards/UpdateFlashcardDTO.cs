namespace FlashcardsProgram.Flashcards;

public class UpdateFlashcardDTO
{
    public string Front { get; set; }
    public string Back { get; set; }

    public UpdateFlashcardDTO(string front, string back)
    {
        Front = front;
        Back = back;
    }
}