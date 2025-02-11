namespace Flashcards.Dreamfxx.Models;
public class Session
{
    public int SessionId { get; set; }
    public int StackId { get; set; }
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public Stack? Stack { get; set; }
}
