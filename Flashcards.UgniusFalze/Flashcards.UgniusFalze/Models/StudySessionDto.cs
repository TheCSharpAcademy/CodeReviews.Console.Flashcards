namespace Flashcards.UgniusFalze.Models;

public class StudySessionDto
{
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public string StackName { get; set; }
    
    public StudySessionDto(DateTime date, int score, string stackName)
    {
        Date = date;
        Score = score;
        StackName = stackName;
    }
}