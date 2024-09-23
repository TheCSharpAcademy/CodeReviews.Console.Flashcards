namespace FlashcardsLibrary.Models;

public class StudySessions
{
    public int StudySessionId { get; set; }
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public string StackName { get; set; }

    public StudySessions(int studySessionId, DateTime date, int score, string stackName)
    {
        StudySessionId = studySessionId;
        Date = date;
        Score = score;
        StackName = stackName;
    }
    public static List<StudySessions> StudySessionsList
    {
        get => DatabaseQueries.Run.SelectAllStudySessions();
    }
}

