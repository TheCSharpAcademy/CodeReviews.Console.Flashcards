namespace FlashcardsProgram.StudySession;

public class StudySessionDAO(
    int id, int numAttempted, int numCorrect, DateTime dateTime, int stackId
)
{
    public int Id { get; set; } = id;
    public DateTime DateTime { get; set; } = dateTime;

    public decimal NumAttempted { get; set; } = numAttempted;
    public decimal NumCorrect { get; set; } = numCorrect;
    public int StackId { get; set; } = stackId;

    public static string TableName
    {
        get
        {
            return "StudySessions";
        }
    }
}