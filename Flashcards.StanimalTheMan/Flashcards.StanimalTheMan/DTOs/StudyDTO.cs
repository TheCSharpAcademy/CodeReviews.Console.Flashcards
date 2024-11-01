namespace Flashcards.StanimalTheMan.DTOs;

internal class StudyDto
{
    public StudyDto(DateTime date, int score, string stackName)
    {
        Date = date;
        Score = score;
        StackName = stackName;
    }

    public DateTime Date { get; set; }
    public int Score { get; set; }
    public string StackName { get; set; }
}
