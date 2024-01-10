namespace Flashcards.StevieTV.Models;

internal class StudySession
{
    public DateTime DateTime { get; set; }
    public Stack Stack { get; set; }
    public int Score { get; set; }
    public int QuantityTested { get; set; }
}