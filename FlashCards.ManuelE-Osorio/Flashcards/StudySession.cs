namespace Flashcards;

class StudySession(string stackName, DateTime date, double score, int studySessionID = 0)
{
    public string StackName = stackName;
    public DateTime Date = date;
    public double Score = score;
    public int StudySessionID = studySessionID;
}