namespace Flashcards;

class FlashcardDto
{
    public long Id { get; }
    public string Front { get; }
    public string Back { get; }

    public FlashcardDto(long id, string front, string back)
    {
        Id = id;
        Front = front;
        Back = back;
    }
}