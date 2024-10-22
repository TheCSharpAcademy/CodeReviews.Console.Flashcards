namespace Flashcards.harris_andy;

public class FlashCardDTO
{
    public string Front { get; set; }
    public string Back { get; set; }

    public FlashCardDTO(string front, string back)
    {
        Front = front;
        Back = back;
    }
}