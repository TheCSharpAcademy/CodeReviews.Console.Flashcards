namespace Flashcards.wkktoria.Models.Dtos;

internal class ReportDataDto
{
    public int StackId { get; set; }
    public string StackName { get; set; } = string.Empty;
    public int SessionYear { get; set; }
    public int SessionMonth { get; set; }
    public int Score { get; set; }

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