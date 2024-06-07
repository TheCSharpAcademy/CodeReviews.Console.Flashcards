namespace Flashcards.Models;

internal class StudySession
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public int Score { get; set; }
    public int TotalQuestions { get; set; }
    public DateTime StudyDate { get; set; }
}