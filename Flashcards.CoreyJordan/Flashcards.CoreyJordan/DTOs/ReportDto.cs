namespace Flashcards.CoreyJordan.DTOs;
internal class ReportDto
{
    public int Sessions { get; set; }
    public double AverageScore { get; set; }
    public DateTime Oldest { get; set; }
    public DateTime Newest { get; set; }

    public ReportDto(List<SessionDto> sessions)
    {
        Sessions = sessions.Count;
        Oldest = sessions.MinBy(x => x.Date)!.Date;
        Newest = sessions.MaxBy(x => x.Date)!.Date;

        double totalScore = 0;
        foreach (SessionDto session in sessions)
        {
            totalScore += session.Score;
        }
        AverageScore = totalScore / sessions.Count;
    }
}
