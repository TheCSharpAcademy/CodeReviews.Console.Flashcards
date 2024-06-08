public interface IConductStudySessions
{
    public void CreateStudySession(int stackId, DateTime date, int score);
    public void GetStudySessions(int stackId);
}