namespace Flashcards.Models;

internal class StudySession
{
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public int Score { get; set; }
    public int QuestionCounter { get; set; }
    public int StackId { get; set; }
}