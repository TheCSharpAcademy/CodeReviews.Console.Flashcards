namespace Flashcards.Models;

public class FlashCard
{
    public int CardID { get; set; }
    public string? Front { get; set; }
    public string? Back { get; set; }
    public int StackID { get; set; }

    public FlashCard() { }

    public FlashCard(string front, string back, int stackID)
    {
        Front = front;
        Back = back;
        StackID = stackID;
    }
}
