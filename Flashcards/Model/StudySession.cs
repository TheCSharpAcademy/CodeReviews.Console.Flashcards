namespace Flashcards.Model
{
    public class StudySession
    {
        public int Id { get; set; }

        public int StackId { get; set; }

        public DateTime SessionStartTime { get; set; }

        public decimal PercentageCorrect { get; set; }

        public string StackName { get; set; }

        public StudySession() { }

        public StudySession(int id, int stackId, DateTime sessionStartTime, decimal percentageCorrect, string stackName)
        {
            Id = id;
            StackId = stackId;
            SessionStartTime = sessionStartTime;
            PercentageCorrect = percentageCorrect;
            StackName = stackName;
        }
    }
}
