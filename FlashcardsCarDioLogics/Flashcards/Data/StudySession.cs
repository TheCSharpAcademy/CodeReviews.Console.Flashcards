
namespace Flashcards.Data;

public class StudySession
{
    public string StacksName { get; set; }
    public string Date { get; set; }
    public int Score { get; set; }

    public StudySession(string stacksName, string date, int score)
    {
        StacksName = stacksName;
        Date = date;
        Score = score;
    }
}
