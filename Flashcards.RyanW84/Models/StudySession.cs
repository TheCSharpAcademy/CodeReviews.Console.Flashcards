namespace Flashcards.RyanW84;

internal class StudySession
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public DateTime Date { get; set; }
    public int Questions { get; set; }
    public int CorrectAnswers { get; set; }
    public int Percentage { get; set; }
    public TimeSpan Time { get; set; }
}
