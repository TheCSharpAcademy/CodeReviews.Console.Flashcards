namespace Flashcards.wkktoria.Models.Dtos;

internal class ReportDataDto
{
    public int StackId { get; init; }
    public string StackName { get; init; } = string.Empty;
    public int SessionYear { get; init; }
    public int SessionMonth { get; init; }
    public int Score { get; init; }

    internal class ReportDataSessions
    {
        public string SessionMonth { get; set; } = string.Empty;
        public int Sessions { get; set; }
    }

    internal class ReportDataAverageScores
    {
        public string SessionMonth { get; set; } = string.Empty;
        public string StackName { get; set; } = string.Empty;
        public int Score { get; set; }
    }
}