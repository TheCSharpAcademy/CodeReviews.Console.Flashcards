namespace FlashcardsProgram.Flashcards;

public class FlashcardDTO(
    int id,
    string front,
    string back
)
{
    public int Id { get; set; } = id;
    public string Front { get; set; } = front;
    public string Back { get; set; } = back;
}