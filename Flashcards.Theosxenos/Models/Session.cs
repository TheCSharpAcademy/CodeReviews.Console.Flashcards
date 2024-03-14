namespace Flashcards.Models;

public class Session
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public int Score { get; set; }
    public DateTime SessionDate { get; set; }
}