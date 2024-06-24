namespace FlashcardsProgram.StudySession;

public class CreateStudySessionDTO(decimal numAttempted, decimal numCorrect, DateTime dateTime, int stackId)
{
    public DateTime DateTime { get; set; } = dateTime;
    public decimal NumAttempted { get; set; } = numAttempted;
    public decimal NumCorrect { get; set; } = numCorrect;
    public int StackId { get; set; } = stackId;
}