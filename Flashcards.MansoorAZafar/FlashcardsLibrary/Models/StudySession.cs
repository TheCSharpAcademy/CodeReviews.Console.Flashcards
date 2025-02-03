namespace FlashcardsLibrary.Models;

internal class StudySession
{
    public static string[] Headers = ["Id", "Score", "Date", "Topic"];
    public int Id { get; set; }
    public int Score { get; set; }
    public DateTime Date { get; set; }
    public string Topic { get; set; }

    public StudySession(int score, DateTime date, string topic)
    {
        this.Score = score;
        this.Date  = date;
        this.Topic = topic;
    }

    public StudySession(int id, int score, DateTime date, string topic)
    {
        this.Id = id;
        this.Score = score;
        this.Date = date;
        this.Topic = topic;
    }
}