namespace FlashcardsProgram.StudySession;

class StudySessionDAO
{
    public DateTime Date { get; set; }
    public decimal Score { get; set; }
    public int StackId { get; set; }

    public static string TableName
    {
        get
        {
            return "StudySessions";
        }
    }

    public StudySessionDAO(DateTime date, decimal score, int stackId)
    {
        Date = date;
        Score = score;
        StackId = stackId;
    }
}