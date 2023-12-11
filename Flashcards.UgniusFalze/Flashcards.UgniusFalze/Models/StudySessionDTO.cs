namespace Flashcards.UgniusFalze.Models;

public class StudySessionDTO
{
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public string StackName { get; set; }
    
    public StudySessionDTO(DateTime date, int score, string stackName)
    {
        Date = date;
        Score = score;
        StackName = stackName;
    }
}