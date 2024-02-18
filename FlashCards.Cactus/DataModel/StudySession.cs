namespace FlashCards.Cactus.DataModel
{
    public class StudySession
    {
        #region Constructors
        public StudySession() { }

        public StudySession(int id, string stackName, DateTime date, double time, int score)
        {
            Id = id;
            StackName = stackName;
            Date = date;
            Time = time;
            Score = score;
        }

        #endregion Constructors

        #region Properties
        public int Id { get; set; }

        public string StackName { get; set; }

        public DateTime Date { get; set; }

        public double Time { get; set; }

        public int Score { get; set; }

        #endregion Properties
    }
}
