namespace Flashcards
{
    internal class Session
    {
        public int StackId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Score { get; set; }
        public int QuestionCount { get; set; }

        public Session(int stackId, string startTime, string endTime, int score, int questionCount)
        {
            StackId = stackId;
            StartTime = startTime;
            EndTime = endTime;
            Score = score;
            QuestionCount = questionCount;
        }

        public List<object> GetField()
        {
            return new List<object> { StackId, StartTime, EndTime, Score, QuestionCount };
        }
    }
}
