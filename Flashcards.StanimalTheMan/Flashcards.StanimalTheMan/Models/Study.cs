namespace Flashcards.StanimalTheMan.Models;

internal class Study
{
    public Study(int studySessionId, DateTime date, int score, int stackId)
    {
        StudySessionId = studySessionId;
        Date = date;
        Score = score;
        StackId = stackId;
    }

    public int StudySessionId { get; set; }
    public DateTime Date { get; set; }

    public int Score { get; set; }

    public int StackId { get; set; }
}
