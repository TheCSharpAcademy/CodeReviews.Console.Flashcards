namespace Flashcards.DAL.Model;

public class Flashcard
{
    public int ID { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
    public int StackID { get; set; }
}
