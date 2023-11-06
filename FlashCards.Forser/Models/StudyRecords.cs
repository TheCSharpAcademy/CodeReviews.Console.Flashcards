namespace FlashCards.Forser.Models
{
    public class StudyRecords
    {
        public int Id { get; set; }
        public DateTime SessionDate { get; set; }
        public string StackName { get; set; }
        public int Score { get; set; }

        public StudyRecords() { }
        public StudyRecords(DateTime sessionDate, string stackName, int score)
        {
            SessionDate = sessionDate;
            StackName = stackName;
            Score = score;
        }
    }
}