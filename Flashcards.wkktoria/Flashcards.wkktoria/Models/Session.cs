namespace Flashcards.wkktoria.Models;

internal class Session
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public DateTime Date { get; set; }
    public int Score { get; set; }
}