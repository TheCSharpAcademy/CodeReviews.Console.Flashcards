namespace FlashCards;

public class StudySessionDto
{
    public string Date { get; set; }
    public int Correct { get; set; }
    public int Total { get; set; }
    public string Name { get; set; }

    public int GetScore()
    {
        return Correct * 100 / Total;
    }
}