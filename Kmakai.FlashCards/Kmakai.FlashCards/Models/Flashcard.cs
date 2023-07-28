namespace Kmakai.FlashCards.Models;

public class Flashcard
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
   
    public Flashcard(int stackId, string front, string back)
    {
        StackId = stackId;
        Front = front;
        Back = back;
    }

}
