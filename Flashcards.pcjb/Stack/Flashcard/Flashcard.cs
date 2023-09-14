namespace Flashcards;

class Flashcard
{
    public long Id { get; }
    public long StackId { get; }
    public string Front { get; }
    public string Back { get; }

    public Flashcard(long id, long stackId, string front, string back)
    {
        Id = id;
        StackId = stackId;
        Front = front;
        Back = back;
    }

    public Flashcard(long stackId, string front, string back)
    {
        StackId = stackId;
        Front = front;
        Back = back;
    }
}