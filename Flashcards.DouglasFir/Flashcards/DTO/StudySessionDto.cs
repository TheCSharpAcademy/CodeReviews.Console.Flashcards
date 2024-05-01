namespace Flashcards.DTO;

public class StudySessionDto
{
    public int StackID { get; set; }
    public DateTime SessionDate { get; set; }
    public int Score { get; set; }

    public StudySessionDto() { }

    public StudySessionDto(int stackID, int score)
    {
        StackID = stackID;
        SessionDate = DateTime.UtcNow;
        Score = score;
    }
}
