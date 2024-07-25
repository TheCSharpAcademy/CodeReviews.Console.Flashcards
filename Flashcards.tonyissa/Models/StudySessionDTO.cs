namespace Flashcards.Models;

public class StudySessionDTO
{
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
    public string StackName { get; set; } = string.Empty;

    public StudySessionDTO()
    {

    }
}