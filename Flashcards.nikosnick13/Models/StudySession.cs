namespace FlashStudy.Models
{
    public class StudySession
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
        public int StackId { get; set; }

        public StudySession(int score, int stackId)
        {
            Score = score;
            StackId = stackId;
            Date = DateTime.Now;
        }
    }
}
