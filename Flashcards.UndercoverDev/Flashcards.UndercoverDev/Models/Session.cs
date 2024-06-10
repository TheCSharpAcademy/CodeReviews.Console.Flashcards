namespace Flashcards.UndercoverDev.Models
{
    public class Session
    {
        public int Id { get; set; }
        public int StackId { get; set; }
        public DateTime SessionDate { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
    }

    public class YearlyStudySessionReport
    {
        public string? StackName { get; set; }
        public Dictionary<string, int> MonthlyScores { get; set; }

        public YearlyStudySessionReport()
        {
            MonthlyScores = new Dictionary<string, int>
            {
                {"January", 0}, {"February", 0}, {"March", 0}, {"April", 0}, {"May", 0}, {"June", 0},
                {"July", 0}, {"August", 0}, {"September", 0}, {"October", 0}, {"November", 0}, {"December", 0}
            };
        }
    }

}