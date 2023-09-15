namespace Flashcards;

class StudySessionHistoryDto
{
    public DateTime CompletedAt { get; set; }
    public string StackName { get; set; }
    public int ScorePercent { get; set; }

    public StudySessionHistoryDto(DateTime completedAt, string stackName, int scorePercent)
    {
        CompletedAt = completedAt;
        StackName = stackName;
        ScorePercent = scorePercent;
    }
}