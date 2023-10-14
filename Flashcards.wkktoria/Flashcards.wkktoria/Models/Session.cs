namespace Flashcards.wkktoria.Models;

internal class Session
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public string Date { get; set; } = string.Empty;
    public int Score { get; set; }
}