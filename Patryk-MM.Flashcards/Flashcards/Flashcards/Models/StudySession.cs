namespace Flashcards.Models;
public class StudySession : BaseEntity {
    public DateTime DateTime { get; set; }
    public int StackId { get; set; }
    public Stack? Stack { get; set; }
    public int TotalQuestions { get; set; }
    public int Score { get; set; }
    public double ScorePercentage => (double)Score / TotalQuestions;

}
