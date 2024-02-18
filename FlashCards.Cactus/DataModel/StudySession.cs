namespace FlashCards.Cactus.DataModel
{
    public class StudySession
    {
        #region Constructors
        public StudySession() { }

        public StudySession(string stackName, double time, int score)
        {
            StackName = stackName;
            Time = time;
            Score = score;
        }

        public StudySession(int sid, string stackName, DateTime date, double time, int score)
        {
            SId = sid;
            StackName = stackName;
            Date = date;
            Time = time;
            Score = score;
        }

        public StudySession(int id, int sid, string stackName, DateTime date, double time, int score)
        {
            Id = id;
            SId = sid;
            StackName = stackName;
            Date = date;
            Time = time;
            Score = score;
        }

        #endregion Constructors

        #region Properties
        public int Id { get; set; }

        public int SId { get; set; }

        public string StackName { get; set; }

        public DateTime Date { get; set; }

        public double Time { get; set; }

        public int Score { get; set; }

        #endregion Properties
    }
}
