namespace FlashcardsProgram.StudySession;

public class StudySessionDAO
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public decimal Score { get; set; }
    public int StackId { get; set; }

    public static string TableName
    {
        get
        {
            return "StudySessions";
        }
    }

    public StudySessionDAO(int id, decimal score, DateTime dateTime, int stackId)
    {
        Id = id;
        DateTime = dateTime;
        Score = score;
        StackId = stackId;
    }
}