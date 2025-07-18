namespace DotNETConsole.Flashcards.Models;


public class Card
{
    public int ID { get; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int STACK_ID { get; }
}
