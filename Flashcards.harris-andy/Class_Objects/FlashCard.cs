namespace Flashcards.harris_andy;

public record FlashCard
{
    public string Front { get; set; }
    public string Back { get; set; }
    public int StackID { get; set; }

    public FlashCard(string front, string back, int stackID)
    {
        Front = front;
        Back = back;
        StackID = stackID;
    }
}