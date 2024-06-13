public class StudySession
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public int TotalQuestions { get; set; }

    public double PercentageScore
    {
        get
        {
            return TotalQuestions > 0 ? (double)Score / TotalQuestions * 100 : 0;
        }
    }
}
