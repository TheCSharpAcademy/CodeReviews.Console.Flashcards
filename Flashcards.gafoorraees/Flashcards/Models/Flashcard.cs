namespace Flashcards.Models;

public class Flashcard
{
    public int ID { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int StackID { get; set; }
    public int DisplayID { get; set; }
}
