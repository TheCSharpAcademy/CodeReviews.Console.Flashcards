namespace Flashcards;

internal class StudySession
{
    public string StackName { get; set; }
    public int StackSize { get; set; }
    public int StackId { get; set; }
    public int NumComplete { get; set; }
    public int NumCorrect { get; set; }
    public double AvgTime { get; set; }
    public DateTime Date { get; set; }
    public static List<StudySession> Sessions { get; set; } = [];

    public StudySession(string StackName, int StackSize, int NumComplete, int NumCorrect, double AvgTime, DateTime Date)
    {
        this.StackName = StackName;
        this.StackSize = StackSize;
        this.NumComplete = NumComplete;
        this.NumCorrect = NumCorrect;
        this.AvgTime = AvgTime;
        this.Date = Date;
        Sessions.Add(this);
    }

    public StudySession(string StackName, int StackSize, int Stack_Id, int NumComplete, int NumCorrect, double AvgTime, string Date)
    {
        this.StackName = StackName;
        this.StackSize = StackSize;
        this.StackId = Stack_Id;
        this.NumComplete = NumComplete;
        this.NumCorrect = NumCorrect;
        this.AvgTime = AvgTime;
        this.Date = DateTime.Parse(Date);
        Sessions.Add(this);
    }
}